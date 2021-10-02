using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using HotChocolate;
using HotChocolate.AspNetCore.Authorization;
using HotChocolate.Types;
using Microsoft.EntityFrameworkCore;
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
                Group = await context.ServiceGroup.FirstAsync(x => x.Id == input.ServiceGroupId, cancellationToken: cancellationToken),
                Parent = await context.Service.FirstAsync(x => x.Id == input.ParentId, cancellationToken: cancellationToken),
                Url = input.Url,
                Status = await context.Status.FirstAsync(x => x.Id == input.StatusId, cancellationToken: cancellationToken)
            };
            return new ServiceBasePayload(service);
        }

        public async Task<ServiceBasePayload> UpdateService(UpdateServiceInput input,
            [ScopedService] ApplicationDbContext context, CancellationToken cancellationToken)
        {
            var service = await context.Service.FirstAsync(x => x.Id == input.ServiceId, cancellationToken: cancellationToken);
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
                service.Parent = await context.Service.FirstAsync(x => x.Id == input.ParentId, cancellationToken: cancellationToken);
            }
            
            await context.SaveChangesAsync(cancellationToken);
            
            return new ServiceBasePayload(service);
        }
        
        public async Task<ServiceBasePayload> DeleteService(DeleteServiceInput input,
            [ScopedService] ApplicationDbContext context, CancellationToken cancellationToken)
        {
            var service = await context.Service.FirstAsync(x => x.Id == input.ServiceId, cancellationToken: cancellationToken);

            await service.Incidents.AsQueryable().DeleteAsync(cancellationToken: cancellationToken);
            await service.Maintenance.AsQueryable().DeleteAsync(cancellationToken: cancellationToken);
            await service.Children.AsQueryable().DeleteAsync(cancellationToken: cancellationToken);
            await service.Monitors.AsQueryable().DeleteAsync(cancellationToken: cancellationToken);
            context.Remove(service);
            await context.BulkSaveChangesAsync(cancellationToken);
            
            return new ServiceBasePayload(service);
        }
    }
}