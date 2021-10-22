using HotChocolate;

namespace Staat.GraphQL.Mutations.Inputs.Status
{
    public record UpdateStatusInput(
        int Id,
        Optional<string> Name,
        Optional<string> Description,
        Optional<string> Color
    );
}