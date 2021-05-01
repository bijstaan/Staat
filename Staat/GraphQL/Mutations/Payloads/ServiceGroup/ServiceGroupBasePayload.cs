using System.Collections.Generic;
using Staat.Helpers;

namespace Staat.GraphQL.Mutations.Payloads.ServiceGroup
{
    public class ServiceGroupBasePayload : Payload
    {
        public Models.ServiceGroup? ServiceGroup { get;}
        public ServiceGroupBasePayload(Models.ServiceGroup serviceGroup)
        {
            ServiceGroup = serviceGroup;
        }
        
        public ServiceGroupBasePayload(IReadOnlyList<UserError> errors)
            : base(errors)
        {
        }

        public ServiceGroupBasePayload(UserError error) : base(new [] { error })
        {
        }
    }
}