using Storage.Net.KeyValue;

namespace Staat.GraphQL.Mutations.Inputs.Setting
{
    public record AddSettingInput(
        string Key,
        string Value);
}