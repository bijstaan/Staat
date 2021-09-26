using HotChocolate.AspNetCore.Authorization;
using HotChocolate.Types;

namespace Staat.GraphQL.Mutations
{
    [ExtendObjectType(OperationTypeNames.Mutation)]
    [Authorize]
    public class ServiceMutation
    {
        
    }
}