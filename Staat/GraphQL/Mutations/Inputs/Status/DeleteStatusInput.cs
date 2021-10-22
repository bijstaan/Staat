using HotChocolate;

namespace Staat.GraphQL.Mutations.Inputs.Status
{
    public record DeleteStatusInput(
        int Id,
        int ReplacementId
    );
}