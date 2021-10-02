#nullable enable
using System.Collections.Generic;
using Staat.Helpers;

namespace Staat.GraphQL.Mutations.Payloads.Service
{
    public class ServiceBasePayload : Payload
    {
        public Models.Service? Service { get;}
        public ServiceBasePayload(Models.Service service)
        {
            Service = service;
        }
        
        public ServiceBasePayload(IReadOnlyList<UserError> errors)
            : base(errors)
        {
        }

        public ServiceBasePayload(UserError error) : base(new [] { error })
        {
        }
    }
}