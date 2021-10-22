namespace Staat.GraphQL.Mutations.Inputs.Status
{
    public record AddStatusInput(
        string Name,
        string Description,
        string Color
    );
}