using System.Collections.Generic;
using HotChocolate;

namespace Staat.GraphQL.Mutations.Inputs.IncidentMessage
{
    public record AddIncidentMessageInput(
        string Message,
        int StatusId,
        int IncidentId,
        Optional<List<int>> AttachedFilesIds
    );
}