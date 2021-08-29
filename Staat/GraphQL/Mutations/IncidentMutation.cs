/*
 * Staat - Staat
 * Copyright (C) 2021 Matthew Kilgore (tankerkiller125)
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
using System.Globalization;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using FluentEmail.Core;
using HotChocolate;
using HotChocolate.AspNetCore.Authorization;
using HotChocolate.Types;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Staat.Data;
using Staat.Extensions;
using Staat.GraphQL.Mutations.Inputs.Incident;
using Staat.GraphQL.Mutations.Payloads.Incident;
using Staat.Helpers;
using Staat.Models;

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
        [UseApplicationContext]
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
                Description = input.Description,
                DescriptionHtml = MarkdownHelper.ToHtml(input.Description),
                Service = await context.Service.FindAsync(input.ServiceId),
                StartedAt = input.StartedAt,
                EndedAt = endedAt,
                Author = context.User.First(x => x.Id == Int32.Parse(_httpContextAccessor.HttpContext!.User.FindFirst(ClaimTypes.Name)!.Value))
            };
            await context.Incident.AddAsync(incident, cancellationToken);
            await context.SaveChangesAsync(cancellationToken);

            var users = context.User.AsQueryable();
            var template = await context.Settings.Where(x => x.Key == "backend.email.template.incident").FirstAsync(cancellationToken: cancellationToken);

            foreach (var user in users)
            {
                await email.To(user.Email).UsingTemplate(template.Value, new
                {
                    Title = incident.Title, 
                    Description = incident.DescriptionHtml, 
                    ServiceName = incident.Service.Name, 
                    StartedAt = incident.StartedAt.ToString(CultureInfo.InvariantCulture), 
                    EndedAt = incident.EndedAt?.ToString(CultureInfo.InvariantCulture)
                }).SendAsync();
            }

            return new IncidentBasePayload(incident);
        }

        public async Task<IncidentBasePayload> UpdateIncidentAsync(UpdateIncidentInput input,
            [ScopedService] ApplicationDbContext context, CancellationToken cancellationToken)
        {
            Incident? incident = await context.Incident.FindAsync(input.Id);
            if (incident is null)
            {
                return new IncidentBasePayload(
                    new UserError("Incident with that id not found.", "INCIDENT_NOT_FOUND"));
            }
            
            if (input.Title.HasValue)
            {
                incident.Title = input.Title;
            }
            
            if (input.Description.HasValue)
            {
                incident.Description = input.Description;
            }
            
            if (input.ServiceId.HasValue)
            {
                incident.Service = await context.Service.FindAsync(input.ServiceId);
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
            return new IncidentBasePayload(incident);
        }
    }
}