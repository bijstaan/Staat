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

#nullable enable
using System;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using FluentEmail.Core;
using HotChocolate;
using HotChocolate.AspNetCore.Authorization;
using HotChocolate.Data;
using HotChocolate.Types;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Staat.Data;
using Staat.Data.Models;
using Staat.GraphQL.Mutations.Inputs.Incident;
using Staat.GraphQL.Mutations.Payloads.Incident;
using Staat.Helpers;
using Z.EntityFramework.Plus;

namespace Staat.GraphQL.Mutations
{
    [ExtendObjectType(OperationTypeNames.Mutation)]
    [Authorize]
    public class IncidentMutation
    {
        private IHttpContextAccessor _httpContextAccessor;
        public IncidentMutation(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }
        
        [UseDbContext(typeof(ApplicationDbContext))]
        public async Task<IncidentBasePayload> AddIncidentAsync(AddIncidentInput input,
            [ScopedService] ApplicationDbContext context, [FromServices] IFluentEmail email, CancellationToken cancellationToken)
        {
            DateTime? endedAt = null;
            if (input.EndedAt.HasValue)
            {
                endedAt = input.EndedAt;
            }
            
            var incident = new Incident
            {
                Title = input.Title,
                Description = input.Description!,
                DescriptionHtml = MarkdownHelper.ToHtml(input.Description),
                Service = await context.Service.DeferredFirst(x => x.Id == input.ServiceId).FromCacheAsync(cancellationToken),
                StartedAt = input.StartedAt,
                EndedAt = endedAt,
                Author = await context.User.DeferredFirst(x => x.Id == Int32.Parse(_httpContextAccessor.HttpContext!.User.FindFirst(ClaimTypes.Name)!.Value)).FromCacheAsync(cancellationToken)
            };
            await context.Incident.AddAsync(incident, cancellationToken);
            await context.SaveChangesAsync(cancellationToken);

            /*var subscribers = context.Subscriber.AsQueryable();
            var template = await context.Settings.Where(x => x.Key == "backend.email.template.incident").FirstAsync(cancellationToken);

            foreach (var subscriber in subscribers)
            {
                await email.To(subscriber.Email).UsingTemplate(template.Value, new
                {
                    Title = incident.Title, 
                    Description = incident.DescriptionHtml, 
                    ServiceName = incident.Service.Name, 
                    StartedAt = incident.StartedAt.ToString(CultureInfo.InvariantCulture), 
                    EndedAt = incident.EndedAt?.ToString(CultureInfo.InvariantCulture),
                    Attachments = incident.Files
                }).SendAsync();
            }*/
            QueryCacheManager.ExpireType<Incident>();
            return new IncidentBasePayload(incident);
        }

        [UseDbContext(typeof(ApplicationDbContext))]
        public async Task<IncidentBasePayload> UpdateIncidentAsync(UpdateIncidentInput input,
            [ScopedService] ApplicationDbContext context, CancellationToken cancellationToken)
        {
            var incident = await context.Incident.DeferredFirst(x => x.Id == input.Id).FromCacheAsync(cancellationToken);
            if (incident is null)
            {
                return new IncidentBasePayload(
                    new UserError("Incident with that id not found.", "INCIDENT_NOT_FOUND"));
            }
            
            if (input.Title.HasValue)
            {
                incident.Title = input.Title!;
            }
            
            if (input.Description.HasValue)
            {
                incident.Description = input.Description!;
            }
            
            if (input.ServiceId.HasValue)
            {
                incident.Service = await context.Service.DeferredFirst(x => x.Id == input.ServiceId).FromCacheAsync(cancellationToken);
            }
            
            if (input.StartedAt.HasValue)
            {
                incident.StartedAt = input.StartedAt;
            }
            
            if (input.EndedAt.HasValue)
            {
                incident.EndedAt = input.EndedAt;
            }
            
            await context.SaveChangesAsync(cancellationToken);
            QueryCacheManager.ExpireType<Incident>();
            return new IncidentBasePayload(incident);
        }
    }
}