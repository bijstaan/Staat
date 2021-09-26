#nullable enable
using System;
using System.Collections.Generic;
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
using Staat.GraphQL.Mutations.Inputs.IncidentMessage;
using Staat.GraphQL.Mutations.Payloads.IncidentMessage;
using Staat.Helpers;
using Staat.Models;
using Z.EntityFramework.Plus;

namespace Staat.GraphQL.Mutations
{
    [ExtendObjectType(OperationTypeNames.Mutation)]
    [Authorize]
    public class IncidentMessageMutation
    {
        private IHttpContextAccessor _httpContextAccessor;

        public IncidentMessageMutation(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        [UseApplicationContext]
        public async Task<IncidentMessageBasePayload> AddIncidentMessage(AddIncidentMessageInput input,
            [ScopedService] ApplicationDbContext context, [FromServices] IFluentEmail email,
            CancellationToken cancellationToken)
        {
            var incident = await context.Incident.IncludeOptimized(x => x.Service)
                .FirstAsync(x => x.Id == input.IncidentId, cancellationToken: cancellationToken);
            var status =
                await context.Status.FirstAsync(x => x.Id == input.StatusId, cancellationToken: cancellationToken);
            var incidentMessage = new IncidentMessage
            {
                Message = input.Message,
                MessageHtml = MarkdownHelper.ToHtml(input.Message),
                Author = await context.User.FirstAsync(
                    x => x.Id == Int32.Parse(_httpContextAccessor.HttpContext!.User.FindFirst(ClaimTypes.Name)!.Value),
                    cancellationToken: cancellationToken),
                Incident = incident,
                Status = status
            };
            if (input.AttachedFilesIds.HasValue)
            {
                incidentMessage.Attachments = await addAttachments(input.AttachedFilesIds.Value, context, cancellationToken);;
            }

            incident.Service.Status = status;

            await context.IncidentMessage.AddAsync(incidentMessage, cancellationToken);
            await context.SaveChangesAsync(cancellationToken);

            /*var subscribers = context.Subscriber.AsQueryable();
            var template = await context.Settings.DeferredFirst(x => x.Key == "backend.email.template.incidentmessage").FromCacheAsync(cancellationToken);
            foreach (var subscriber in subscribers)
            {
                await email.To(subscriber.Email)
                    .UsingTemplate(template.Value, new
                {
                    Message = incidentMessage.MessageHtml, 
                    ServiceName = incidentMessage.Incident.Service.Name, 
                    StartedAt = incidentMessage.CreatedAt.ToString(CultureInfo.InvariantCulture),
                    Attachments = incidentMessage.Attachments
                }).SendAsync();
            }*/

            return new IncidentMessageBasePayload(incidentMessage);
        }

        public async Task<IncidentMessageBasePayload> UpdateIncidentMessage(UpdateIncidentMessageInput input,
            [ScopedService] ApplicationDbContext context, CancellationToken cancellationToken)
        {
            IncidentMessage? incidentMessage = await context.IncidentMessage
                .Include(x => x.Incident)
                .ThenInclude(x => x.Service)
                .FirstAsync(x => x.Id == input.MessageId, cancellationToken: cancellationToken);
            if (incidentMessage is null)
            {
                return new IncidentMessageBasePayload(
                    new UserError("Incident with that id not found.", "INCIDENT_MESSAGE_NOT_FOUND"));
            }

            if (input.StatusId.HasValue)
            {
                var status = await context.Status.FirstAsync(x => x.Id == input.StatusId,
                    cancellationToken: cancellationToken);
                incidentMessage.Status = status;
                incidentMessage.Incident.Service.Status = status;
            }

            if (input.Message.HasValue)
            {
                incidentMessage.Message = input.Message;
                incidentMessage.MessageHtml = MarkdownHelper.ToHtml(input.Message);
            }

            if (input.IncidentId.HasValue)
            {
                var incident = await context.Incident.FirstAsync(x => x.Id == input.IncidentId,
                    cancellationToken: cancellationToken);
                incidentMessage.Incident = incident;
            }

            if (input.AttachedFilesIds.HasValue)
            {
                incidentMessage.Attachments = await addAttachments(input.AttachedFilesIds.Value, context, cancellationToken);;
            }

            await context.SaveChangesAsync(cancellationToken);
            return new IncidentMessageBasePayload(new IncidentMessage());
        }

        public async Task<IncidentMessageBasePayload> DeleteIncidentMessage(DeleteIncidentMessageInput input,
            [ScopedService] ApplicationDbContext context, CancellationToken cancellationToken)
        {
            IncidentMessage? incidentMessage = await context.IncidentMessage.FirstAsync(x => x.Id == input.MessageId,
                cancellationToken: cancellationToken);
            if (incidentMessage is null)
            {
                return new IncidentMessageBasePayload(
                    new UserError("Incident with that id not found.", "INCIDENT_MESSAGE_NOT_FOUND"));
            }
            context.Remove(incidentMessage);
            await context.SaveChangesAsync(cancellationToken);
            return new IncidentMessageBasePayload(incidentMessage);
        }

        private async Task<List<File>> addAttachments(List<int>? attachedFilesIds, ApplicationDbContext context, CancellationToken cancellationToken)
        {
            var attachments = new List<File>();
            if (attachedFilesIds != null)
                foreach (var attachedFileId in attachedFilesIds)
                {
                    var file = await context.File.FirstAsync(x => x.Id == attachedFileId,
                        cancellationToken: cancellationToken);
                    attachments.Add(file);
                }
            return attachments;
        }
    }
}