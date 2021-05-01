using HotChocolate;

namespace Staat.GraphQL.Mutations.Inputs.ServiceGroup
{
    public record UpdateServiceGroupInput(
        int Id,
        Optional<string?> Name,
        Optional<string?> Description,
        bool? DefaultOpen);
}