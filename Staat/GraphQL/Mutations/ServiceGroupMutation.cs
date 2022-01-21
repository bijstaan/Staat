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
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using HotChocolate;
using HotChocolate.AspNetCore.Authorization;
using HotChocolate.Data;
using HotChocolate.Types;
using Microsoft.EntityFrameworkCore;
using NetBox.Extensions;
using Staat.Data;
using Staat.Extensions;
using Staat.GraphQL.Mutations.Inputs.ServiceGroup;
using Staat.GraphQL.Mutations.Payloads.ServiceGroup;
using Staat.Helpers;
using Staat.Models;
using Z.EntityFramework.Plus;

namespace Staat.GraphQL.Mutations
{
    [ExtendObjectType(OperationTypeNames.Mutation)]
    [Authorize]
    public class ServiceGroupMutation
    {
        [UseDbContext(typeof(ApplicationDbContext))]
        public async Task<ServiceGroupBasePayload> AddServiceGroupAsync(AddServiceGroupInput input, [ScopedService] ApplicationDbContext context, CancellationToken cancellationToken)
        {
            var serviceGroup = new ServiceGroup
            {
                Name = input.Name,
                Description = input.Description,
                _DefaultOpen = input.DefaultOpen,
            };
            await context.ServiceGroup.AddAsync(serviceGroup, cancellationToken);
            await context.SaveChangesAsync(cancellationToken);
            QueryCacheManager.ExpireType<ServiceGroup>();
            return new ServiceGroupBasePayload(serviceGroup);
        }
        [UseDbContext(typeof(ApplicationDbContext))]
        public async Task<ServiceGroupBasePayload> UpdateServiceGroupAsync(UpdateServiceGroupInput input,
            [ScopedService] ApplicationDbContext context, CancellationToken cancellationToken)
        {
            var serviceGroup = await context.ServiceGroup.DeferredFirst(x => x.Id == input.Id).FromCacheAsync(cancellationToken);
            if (serviceGroup is null)
            {
                return new ServiceGroupBasePayload(
                    new UserError("Service Group with id not found.", "SERVICE_GROUP_NOT_FOUND"));
            }

            if (input.Name.HasValue)
            {
                serviceGroup.Name = input.Name;
            }

            if (input.Description.HasValue)
            {
                serviceGroup.Description = input.Description;
            }

            if (input.DefaultOpen.HasValue)
            {
                serviceGroup._DefaultOpen = (bool) input.DefaultOpen!;
            }
            
            await context.SaveChangesAsync(cancellationToken);
            QueryCacheManager.ExpireType<ServiceGroup>();
            return new ServiceGroupBasePayload(serviceGroup);
        }

        [UseDbContext(typeof(ApplicationDbContext))]
        public async Task<ServiceGroupBasePayload> DeleteServiceGroup(DeleteServiceGroupInput input,
            [ScopedService] ApplicationDbContext context, CancellationToken cancellationToken)
        {
            var serviceGroup = await context.ServiceGroup.DeferredFirst(x => x.Id == input.ServiceGroupId).FromCacheAsync(cancellationToken);
            var replacementGroup = await context.ServiceGroup.DeferredFirst(x => x.Id == input.ReplacementGroupId).FromCacheAsync(cancellationToken);
            var services = await context.Service.Where(x => x.Group == serviceGroup).FromCacheAsync(cancellationToken);
            foreach (var service in services)
            {
                service.Group = replacementGroup;
            }
            context.ServiceGroup.Remove(serviceGroup);
            await context.SaveChangesAsync(cancellationToken);
            QueryCacheManager.ExpireType<ServiceGroup>();
            return new ServiceGroupBasePayload(serviceGroup);
        }
    }
}