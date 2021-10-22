using System.Collections.Generic;
using Staat.Helpers;

namespace Staat.GraphQL.Mutations.Payloads.Status
{
    public class StatusBasePayload : Payload
    {
        public Models.Status? Status { get;}
        
        public StatusBasePayload(Models.Status settings)
        {
            Status = settings;
        }
        
        public StatusBasePayload(IReadOnlyList<UserError> errors)
            : base(errors)
        {
        }
        
        public StatusBasePayload(UserError error) : base(new [] { error })
        {
        }
    }
}