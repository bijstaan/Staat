using System.Collections.Generic;
using HotChocolate;

namespace Staat.GraphQL.Mutations.Inputs.IncidentMessage
{
    public record UpdateIncidentMessageInput(
        int MessageId,
        Optional<string> Message,
        Optional<int> StatusId,
        Optional<int> IncidentId,
        Optional<List<int>> AttachedFilesIds
        );
}