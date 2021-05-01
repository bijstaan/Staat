using System.Threading;
using System.Threading.Tasks;
using HotChocolate;
using HotChocolate.Types;
using Staat.Data;
using Staat.Extensions;
using Staat.GraphQL.Mutations.Inputs.ServiceGroup;
using Staat.GraphQL.Mutations.Payloads.ServiceGroup;
using Staat.Helpers;
using Staat.Models;

namespace Staat.GraphQL.Mutations
{
    [ExtendObjectType(Name = "Mutation")]
    public class ServiceGroupMutation
    {
        [UseApplicationContext]
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
        public async Task<ServiceGroupBasePayload> UpdateServiceGroupAsync(UpdateServiceGroupInput input,
            [ScopedService] ApplicationDbContext context, CancellationToken cancellationToken)
        {
            ServiceGroup? serviceGroup = await context.ServiceGroup.FindAsync(input.Id);
            if (serviceGroup is null)
            {
                return new ServiceGroupBasePayload(
                    new UserError("Speaker with id not found.", "SPEAKER_NOT_FOUND"));
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