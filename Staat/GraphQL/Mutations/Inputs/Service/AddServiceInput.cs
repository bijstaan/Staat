using HotChocolate;

namespace Staat.GraphQL.Mutations.Inputs.Service
{
    public record AddServiceInput
    (
        string Name,
        Optional<string> Description,
        Optional<string> Url,
        int ServiceGroupId,
        Optional<int> ParentId,
        int StatusId
    );
}