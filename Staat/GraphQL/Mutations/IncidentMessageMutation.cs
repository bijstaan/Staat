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
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
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
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Staat.Data;
using Staat.Data.Models;
using Staat.GraphQL.Mutations.Inputs.IncidentMessage;
using Staat.GraphQL.Mutations.Payloads.IncidentMessage;
using Staat.Helpers;
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

        [UseDbContext(typeof(ApplicationDbContext))]
        public async Task<IncidentMessageBasePayload> AddIncidentMessage(AddIncidentMessageInput input,
            [ScopedService] ApplicationDbContext context, [FromServices] IFluentEmail email,
            CancellationToken cancellationToken)
        {
            var incident = await context.Incident.IncludeOptimized(x => x.Service).IncludeOptimized(x => x.Messages)
                .DeferredFirst(x => x.Id == input.IncidentId).FromCacheAsync(cancellationToken);
            var status =
                await context.Status.DeferredFirst(x => x.Id == input.StatusId).FromCacheAsync(cancellationToken);
            var incidentMessage = new IncidentMessage
            {
                Message = input.Message,
                MessageHtml = MarkdownHelper.ToHtml(input.Message),
                Author = await context.User.DeferredFirst(
                    x => x.Id == Int32.Parse(_httpContextAccessor.HttpContext!.User.FindFirst(ClaimTypes.Name)!.Value)).FromCacheAsync(cancellationToken),
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
            
            var emailDomain = await context.Settings.DeferredFirst(x => x.Key == "backend.email.domain").FromCacheAsync(cancellationToken);
            var template = await context.Settings.DeferredFirst(x => x.Key == "backend.email.template.incidentmessage").FromCacheAsync(cancellationToken);
            try
            {
                await context.Subscriber.ForEachAsync((subscriber) =>
                {
                    if (!template.Value.IsNullOrEmpty())
                    {
                        email.To(subscriber.Email).Subject($"Incident Created: {incident.Service.Name}").UsingTemplate(template.Value, new
                        {
                            Title = incident.Title,
                            Description = incident.Description,
                            ServiceName = incident.Service.Name,
                            StartedAd = incident.StartedAt.ToString(CultureInfo.InvariantCulture),
                            EndedAt = incident.EndedAt?.ToString(CultureInfo.InvariantCulture),
                            Attachements = incident.Files
                        });
                        // Build references list from previous incident messages, newest first, oldest last
                        var references = "";
                        incident.Messages.OrderByDescending(x => x.CreatedAt).ForEach((message) =>
                        {
                            references += $"<{message.Id}+{incident.Id}@{emailDomain.Value}> ";
                        });
                        // We set the message ID so we can reference it in status updates and what not.
                        email.Header("Message-ID", $"<{incidentMessage.Id}+{incident.Id}@{emailDomain.Value}>");
                        // Build references string for header
                        email.Header("References", $"<{incidentMessage.Id}+{incident.Id}@{emailDomain.Value}> {references}<{incident.Id}@{emailDomain.Value}>");
                        email.Send();
                    }
                }, cancellationToken);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            QueryCacheManager.ExpireType<IncidentMessage>();
            return new IncidentMessageBasePayload(incidentMessage);
        }

        [UseDbContext(typeof(ApplicationDbContext))]
        public async Task<IncidentMessageBasePayload> UpdateIncidentMessage(UpdateIncidentMessageInput input,
            [ScopedService] ApplicationDbContext context, CancellationToken cancellationToken)
        {
            var incidentMessage = await context.IncidentMessage
                .Include(x => x.Incident)
                .ThenInclude(x => x.Service)
                .DeferredFirst(x => x.Id == input.MessageId).FromCacheAsync(cancellationToken);
            if (incidentMessage is null)
            {
                return new IncidentMessageBasePayload(
                    new UserError("Incident with that id not found.", "INCIDENT_MESSAGE_NOT_FOUND"));
            }

            if (input.StatusId.HasValue)
            {
                var status = await context.Status.DeferredFirst(x => x.Id == input.StatusId).FromCacheAsync(cancellationToken);
                incidentMessage.Status = status;
                incidentMessage.Incident.Service.Status = status;
            }

            if (input.Message.HasValue)
            {
                incidentMessage.Message = input.Message!;
                incidentMessage.MessageHtml = MarkdownHelper.ToHtml(input.Message);
            }

            if (input.IncidentId.HasValue)
            {
                var incident = await context.Incident.DeferredFirst(x => x.Id == input.IncidentId).FromCacheAsync(cancellationToken);
                incidentMessage.Incident = incident;
            }

            if (input.AttachedFilesIds.HasValue)
            {
                incidentMessage.Attachments = await addAttachments(input.AttachedFilesIds.Value, context, cancellationToken);;
            }
            await context.BulkSaveChangesAsync(cancellationToken);
            QueryCacheManager.ExpireType<IncidentMessage>();
            return new IncidentMessageBasePayload(incidentMessage);
        }

        [UseDbContext(typeof(ApplicationDbContext))]
        public async Task<IncidentMessageBasePayload> DeleteIncidentMessage(DeleteIncidentMessageInput input,
            [ScopedService] ApplicationDbContext context, CancellationToken cancellationToken)
        {
            var incidentMessage = await context.IncidentMessage.DeferredFirst(x => x.Id == input.MessageId).FromCacheAsync(cancellationToken);
            if (incidentMessage is null)
            {
                return new IncidentMessageBasePayload(
                    new UserError("Incident with that id not found.", "INCIDENT_MESSAGE_NOT_FOUND"));
            }
            context.Remove(incidentMessage);
            await context.SaveChangesAsync(cancellationToken);
            QueryCacheManager.ExpireType<IncidentMessage>();
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