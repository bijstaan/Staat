using HotChocolate;

namespace Staat.GraphQL.Mutations.Inputs.Service
{
    public record UpdateServiceInput(
        int ServiceId,
        Optional<string> Name,
        Optional<string> Description,
        Optional<string> Url,
        Optional<int> ParentId
    );
}