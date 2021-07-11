using System;
using HotChocolate;

namespace Staat.GraphQL.Mutations.Inputs.Incident
{
    public record UpdateIncidentInput(
        int Id,
        Optional<string> Title, 
        Optional<string> Description, 
        Optional<int> ServiceId, 
        Optional<DateTime> StartedAt,
        Optional<DateTime?> EndedAt);
}