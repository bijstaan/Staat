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

using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using HotChocolate;
using HotChocolate.AspNetCore.Authorization;
using HotChocolate.Data;
using HotChocolate.Types;
using Staat.Data;
using Staat.Data.Models;
using Staat.GraphQL.Mutations.Inputs.Status;
using Staat.GraphQL.Mutations.Payloads.Status;
using Staat.Helpers;
using Z.EntityFramework.Plus;

namespace Staat.GraphQL.Mutations
{
    [ExtendObjectType(OperationTypeNames.Mutation)]
    [Authorize]
    public class StatusMutation
    {
        [UseDbContext(typeof(ApplicationDbContext))]
        public async Task<StatusBasePayload> AddStatus(AddStatusInput input,
            [ScopedService] ApplicationDbContext context, CancellationToken cancellationToken)
        {
            var status = new Status
            {
                Name = input.Name,
                Description = input.Description,
                Color = input.Color
            };
            await context.Status.AddAsync(status, cancellationToken);
            await context.SaveChangesAsync(cancellationToken);
            QueryCacheManager.ExpireType<Status>();
            return new StatusBasePayload(status);
        }
        
        [UseDbContext(typeof(ApplicationDbContext))]
        public async Task<StatusBasePayload> UpdateStatus(UpdateStatusInput input,
            [ScopedService] ApplicationDbContext context, CancellationToken cancellationToken)
        {
            var status = await context.Status.DeferredFirst(x => x.Id == input.Id).FromCacheAsync(cancellationToken);
            if (status is null)
            {
                return new StatusBasePayload(
                    new UserError("Status with that id not found.", "STATUS_NOT_FOUND"));
            }

            if (input.Description.HasValue)
            {
                status.Description = input.Description;
            }

            if (input.Color.HasValue)
            {
                status.Color = input.Color;
            }

            if (input.Name.HasValue)
            {
                status.Name = input.Name;
            }
            QueryCacheManager.ExpireType<Status>();
            return new StatusBasePayload(status);
        }
        
        [UseDbContext(typeof(ApplicationDbContext))]
        public async Task<StatusBasePayload> DeleteStatus(DeleteStatusInput input,
            [ScopedService] ApplicationDbContext context, CancellationToken cancellationToken)
        {
            var status = await context.Status.DeferredFirst(x => x.Id == input.Id).FromCacheAsync(cancellationToken);
            var replacementStatus = await context.Status.DeferredFirst(x => x.Id == input.ReplacementId).FromCacheAsync(cancellationToken);
            if (status is null || replacementStatus is null)
            {
                return new StatusBasePayload(
                    new UserError("Status with that id not found.", "STATUS_NOT_FOUND"));
            }
            var services = await context.Service.Where(x => x.Status == status).IncludeOptimized(x => x.Status).FromCacheAsync();
            var incidentMessages = await context.IncidentMessage.Where(x => x.Status == status).IncludeOptimized(x => x.Status).FromCacheAsync();
            foreach (var service in services)
            {
                service.Status = replacementStatus;
            }
            foreach (var incidentMessage in incidentMessages)
            {
                incidentMessage.Status = replacementStatus;
            }
            context.Status.Remove(status);
            await context.BulkSaveChangesAsync(cancellationToken);
            QueryCacheManager.ExpireType<Status>();
            return new StatusBasePayload(status);
        }
    }
}