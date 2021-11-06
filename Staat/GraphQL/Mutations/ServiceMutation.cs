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
using HotChocolate.Types;
using Staat.Data;
using Staat.Extensions;
using Staat.GraphQL.Mutations.Inputs.Service;
using Staat.GraphQL.Mutations.Payloads.Service;
using Staat.Helpers;
using Staat.Models;
using Z.EntityFramework.Plus;

namespace Staat.GraphQL.Mutations
{
    [ExtendObjectType(OperationTypeNames.Mutation)]
    [Authorize]
    public class ServiceMutation
    {

        [UseApplicationContext]
        public async Task<ServiceBasePayload> AddService(AddServiceInput input, [ScopedService] ApplicationDbContext context, CancellationToken cancellationToken)
        {
            var service = new Service
            {
                Name = input.Name,
                Description = input.Description,
                Group = await context.ServiceGroup.DeferredFirst(x => x.Id == input.ServiceGroupId).FromCacheAsync(cancellationToken),
                Parent = await context.Service.DeferredFirst(x => x.Id == input.ParentId).FromCacheAsync(cancellationToken),
                Url = input.Url,
                Status = await context.Status.DeferredFirst(x => x.Id == input.StatusId).FromCacheAsync(cancellationToken)
            };
            context.Service.Add(service);
            await context.SaveChangesAsync(cancellationToken);
            QueryCacheManager.ExpireType<Service>();
            return new ServiceBasePayload(service);
        }

        [UseApplicationContext]
        public async Task<ServiceBasePayload> UpdateService(UpdateServiceInput input,
            [ScopedService] ApplicationDbContext context, CancellationToken cancellationToken)
        {
            var service = await context.Service.DeferredFirst(x => x.Id == input.ServiceId).FromCacheAsync(cancellationToken);
            if (service is null)
            {
                return new ServiceBasePayload(
                    new UserError("Service with that id not found.", "SERVICE_NOT_FOUND"));
            }
            if (input.Name.HasValue)
            {
                service.Name = input.Name;
            }
            
            if (input.Description.HasValue)
            {
	            service.Description = input.Description; 
            }

            if (input.Url.HasValue)
            {
                service.Url = input.Url;
            }

            if (input.ParentId.HasValue)
            {
                service.Parent = await context.Service.DeferredFirst(x => x.Id == input.ParentId).FromCacheAsync(cancellationToken);
            }
            
            await context.SaveChangesAsync(cancellationToken);
            QueryCacheManager.ExpireType<Service>();
            return new ServiceBasePayload(service);
        }
        
        [UseApplicationContext]
        public async Task<ServiceBasePayload> DeleteService(DeleteServiceInput input,
            [ScopedService] ApplicationDbContext context, CancellationToken cancellationToken)
        {
            var service = await context.Service
                .IncludeOptimized(x => x.Incidents)
                .IncludeOptimized(x => x.Maintenance)
                .IncludeOptimized(x => x.Children)
                .IncludeOptimized(x => x.Monitors)
                .DeferredFirst(x => x.Id == input.ServiceId).FromCacheAsync(cancellationToken);
            await service.Incidents.AsQueryable().DeleteAsync(cancellationToken: cancellationToken);
            await service.Maintenance.AsQueryable().DeleteAsync(cancellationToken: cancellationToken);
            await service.Children.AsQueryable().DeleteAsync(cancellationToken: cancellationToken);
            await service.Monitors.AsQueryable().DeleteAsync(cancellationToken: cancellationToken);
            context.Remove(service);
            await context.BulkSaveChangesAsync(cancellationToken);
            QueryCacheManager.ExpireType<Service>();
            return new ServiceBasePayload(service);
        }
    }
}