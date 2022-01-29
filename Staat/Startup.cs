/*
 * Staat - Staat
 * Copyright (C) 2021 Bijstaan
 *
 * This program is free software: you can redistribute it and/or modify
 * it under the terms of the GNU Affero General Public License as published
 * by the Free Software Foundation, either version 3 of the License, or (at your
 * option) any later version.
 *
 * This program is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU Affero General Public License for more details.
 *
 * You should have received a copy of the GNU Affero General Public License
 * along with this program.  If not, see <https://www.gnu.org/licenses/>.
 */

using System;
using System.Text;
using System.Threading.Tasks;
using Coravel;
using FluentEmail.MailKitSmtp;
using HotChocolate.Language;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.IdentityModel.Tokens;
using Sentry.AspNetCore;
using Staat.Data;
using Staat.GraphQL.Mutations;
using Staat.Services;
using Staat.GraphQL.Queries;
using Staat.GraphQL.Subscriptions;
using Staat.Jobs;

namespace Staat
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            /*
             * Database Section
             */
            try
            {
                var provider = Configuration.GetSection("App")["DatabaseType"].ToUpper();
                services.AddDbContextFactory<ApplicationDbContext>(options => _ = provider switch
                {
                    "NPGSQL" => options.UseNpgsql(
                        Configuration.GetConnectionString("DefaultConnection"),
                        x => x.MigrationsAssembly("NpgsqlMigrations")),

                    "SQLSERVER" => options.UseSqlServer(
                        Configuration.GetConnectionString("DefaultConnection"),
                        x => x.MigrationsAssembly("SqlServerMigrations")),

                    _ => throw new Exception($"Unsupported provider: {provider}")
                });
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }

            /*
             * Email configuration
             */
            var emailConfig = Configuration.GetSection("Email");
            services.AddFluentEmail(emailConfig["Address"], emailConfig["Name"])
                .AddLiquidRenderer().AddMailKitSender(new SmtpClientOptions
                {
                    Server = emailConfig["Host"],
                    Port = int.Parse(emailConfig["Port"]),
                    User = emailConfig["Username"],
                    Password = emailConfig["Password"],
                    UseSsl = bool.Parse(emailConfig["UseSsl"]),
                });

            /*
             * Cache Section
             */
            services.AddMemoryCache();
            services.AddSha256DocumentHashProvider(HashFormat.Hex);

            /*
             * Coravel Section
             */
            services.AddScheduler();
            services.AddQueue();
            services.AddTransient<CheckForJobs>();

            /*
             * GraphQL Section
             */
            services.AddGraphQLServer()
                .AddAuthorization()
                .AddProjections()
                .AddFiltering()
                .AddSorting()
                .AddInMemorySubscriptions()
                .AddApolloTracing()
                .AddQueryType(d => d.Name("Query"))
                .AddTypeExtension<IncidentQuery>()
                .AddTypeExtension<MaintenanceQuery>()
                .AddTypeExtension<MonitorQuery>()
                .AddTypeExtension<ServiceGroupQuery>()
                .AddTypeExtension<ServiceQuery>()
                .AddTypeExtension<SettingsQuery>()
                .AddTypeExtension<UserQuery>()
                .AddMutationType(d => d.Name("Mutation"))
                .AddTypeExtension<IncidentMutation>()
                .AddTypeExtension<IncidentMessageMutation>()
                //.AddTypeExtension<MaintenanceMutation>()
                //.AddTypeExtension<MaintenanceMessageMutation>()
                //.AddTypeExtension<MonitorMutation>()
                .AddTypeExtension<ServiceGroupMutation>()
                .AddTypeExtension<ServiceMutation>()
                .AddTypeExtension<SettingMutation>()
                .AddTypeExtension<StatusMutation>()
                .AddSubscriptionType(d => d.Name("Subscription"))
                .AddTypeExtension<ServiceSubscription>()
                .UseAutomaticPersistedQueryPipeline()
                .AddInMemoryQueryStorage();

            /*
             * MISC. Area
             */
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IFileService, FileService>();
            services.AddScoped<DbContext, ApplicationDbContext>();

            services.AddHttpContextAccessor();

            /*
             * Authentication Area
             */
            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(x =>
            {
                x.ClaimsIssuer = Configuration.GetSection("JwtBearer")["Issuer"];
                x.Audience = Configuration.GetSection("JwtBearer")["Audience"];
                x.Authority = Configuration.GetSection("JwtBearer")["Authority"];
                x.Events = new JwtBearerEvents
                {
                    OnTokenValidated = context =>
                    {
                        var userService = context.HttpContext.RequestServices.GetRequiredService<IUserService>();
                        var userId = int.Parse(context.Principal?.Identity?.Name ?? "0");
                        var user = userService.GetById(userId);
                        if (user == null)
                        {
                            context.Fail("Unauthorized");
                        }

                        return Task.CompletedTask;
                    }
                };
                var key = Encoding.ASCII.GetBytes(Configuration.GetSection("JwtBearer")["Secret"]);
                x.RequireHttpsMetadata = true;
                x.SaveToken = true;
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ClockSkew = TimeSpan.FromMinutes(1),
                    ValidIssuer = Configuration.GetSection("JwtBearer")["Issuer"],
                    ValidAudience = Configuration.GetSection("JwtBearer")["Audience"],
                };
            });

            /*
             * Routing Section
             */
            services.AddControllers();
            services.AddSpaStaticFiles(configuration =>
            {
                configuration.RootPath = "WebApp/dist";
            });
            services.AddSwaggerGen(c => { c.SwaggerDoc("v1", new OpenApiInfo {Title = "Staat", Version = "v1"}); });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IDbContextFactory<ApplicationDbContext> context)
        {
            // Migrate database if it hasn't already been done or there are updates
            context.CreateDbContext().Database.Migrate();
            
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Staat v1"));
            }

            if (bool.Parse(Configuration.GetSection("App")["RedirectToHttps"]))
            {
                app.UseHttpsRedirection();
            }

            app.UseRouting();

            app.UseSentryTracing();
            
            app.UseDefaultFiles();
            app.UseStaticFiles();
            if (!env.IsDevelopment())
            {
                app.UseSpaStaticFiles();
            }

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseWebSockets();
            app.UseEndpoints(endpoints => { 
                endpoints.MapControllers();
                endpoints.MapGraphQL("/api/graphql");
            });
            
            app.UseSpa(spa =>
            {
                spa.Options.SourcePath = "WebApp";

                if (env.IsDevelopment())
                {
                    // if you just prefer to proxy requests from client app, use proxy to SPA dev server instead,
                    // app should be already running before starting a .NET client:
                    // run npm process with client app
                    spa.UseProxyToSpaDevelopmentServer($"http://localhost:8000");
                }
            });
            
            var provider = app.ApplicationServices;
            provider.ConfigureQueue().OnError(e =>
            {
                Console.WriteLine(e.StackTrace);
            });
            provider.UseScheduler(scheduler =>
            {
                scheduler.Schedule<CheckForJobs>().EveryFiveSeconds();
            });
        }
    }
}