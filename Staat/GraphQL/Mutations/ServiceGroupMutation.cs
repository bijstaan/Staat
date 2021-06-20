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
using System.Threading;
using System.Threading.Tasks;
using HotChocolate;
using HotChocolate.AspNetCore.Authorization;
using HotChocolate.Types;
using Staat.Data;
using Staat.Extensions;
using Staat.GraphQL.Mutations.Inputs.ServiceGroup;
using Staat.GraphQL.Mutations.Payloads.ServiceGroup;
using Staat.Helpers;
using Staat.Models;

namespace Staat.GraphQL.Mutations
{
    [ExtendObjectType(OperationTypeNames.Mutation)]
    [Authorize]
    public class ServiceGroupMutation
    {
        [UseApplicationContext]
        [Authorize]
        public async Task<ServiceGroupBasePayload> AddServiceGroupAsync(AddServiceGroupInput input, [ScopedService] ApplicationDbContext context, CancellationToken cancellationToken)
        {
            var serviceGroup = new ServiceGroup
            {
                Name = input.Name,
                Description = input.Description,
                DefaultOpen = input.DefaultOpen,
            };
            await context.ServiceGroup.AddAsync(serviceGroup, cancellationToken);
            await context.SaveChangesAsync(cancellationToken);
            return new ServiceGroupBasePayload(serviceGroup);
        }
        [UseApplicationContext]
        [Authorize]
        public async Task<ServiceGroupBasePayload> UpdateServiceGroupAsync(UpdateServiceGroupInput input,
            [ScopedService] ApplicationDbContext context, CancellationToken cancellationToken)
        {
            ServiceGroup? serviceGroup = await context.ServiceGroup.FindAsync(input.Id);
            if (serviceGroup is null)
            {
                return new ServiceGroupBasePayload(
                    new UserError("Service Group with id not found.", "SPEAKER_NOT_FOUND"));
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
                serviceGroup.DefaultOpen = input.DefaultOpen.Value;
            }
            
            await context.SaveChangesAsync(cancellationToken);
            return new ServiceGroupBasePayload(serviceGroup);
        }
    }
}