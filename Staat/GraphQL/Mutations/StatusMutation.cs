using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using HotChocolate;
using HotChocolate.AspNetCore.Authorization;
using HotChocolate.Types;
using Microsoft.EntityFrameworkCore;
using Staat.Data;
using Staat.Extensions;
using Staat.GraphQL.Mutations.Inputs.Status;
using Staat.GraphQL.Mutations.Payloads.Status;
using Staat.Helpers;
using Staat.Models;
using Z.EntityFramework.Plus;

namespace Staat.GraphQL.Mutations
{
    [ExtendObjectType(OperationTypeNames.Mutation)]
    [Authorize]
    public class StatusMutation
    {
        [UseApplicationContext]
        public async Task<StatusBasePayload> AddStatus(AddStatusInput input,
            [ScopedService] ApplicationDbContext context, CancellationToken cancellationToken)
        {
            var status = new Status
            {
                Name = input.Name,
                Description = input.Description,
                Color = input.Color
            };
            context.Status.Add(status);
            await context.SaveChangesAsync(cancellationToken);
            return new StatusBasePayload(status);
        }
        
        public async Task<StatusBasePayload> UpdateStatus(UpdateStatusInput input,
            [ScopedService] ApplicationDbContext context, CancellationToken cancellationToken)
        {
            var status = await context.Status.FirstAsync(x => x.Id == input.Id, cancellationToken: cancellationToken);
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
            return new StatusBasePayload(status);
        }
        
        public async Task<StatusBasePayload> DeleteStatus(DeleteStatusInput input,
            [ScopedService] ApplicationDbContext context, CancellationToken cancellationToken)
        {
            var status = await context.Status.FirstAsync(x => x.Id == input.Id);
            var replacementStatus = await context.Status.FirstAsync(x => x.Id == input.ReplacementId);
            if (status is null || replacementStatus is null)
            {
                return new StatusBasePayload(
                    new UserError("Status with that id not found.", "STATUS_NOT_FOUND"));
            }
            var services = context.Service.Where(x => x.Status == status).IncludeOptimized(x => x.Status).AsQueryable();
            var incidentMessages = context.IncidentMessage.Where(x => x.Status == status).IncludeOptimized(x => x.Status).AsQueryable();
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
            return new StatusBasePayload(status);
        }
    }
}