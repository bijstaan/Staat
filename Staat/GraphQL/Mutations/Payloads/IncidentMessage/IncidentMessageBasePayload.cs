#nullable enable
using System.Collections.Generic;
using Staat.Helpers;

namespace Staat.GraphQL.Mutations.Payloads.IncidentMessage
{
    public class IncidentMessageBasePayload : Payload
    {
        public Models.IncidentMessage? IncidentMessage { get;}
        
        public IncidentMessageBasePayload(Models.IncidentMessage incidentMessage)
        {
            IncidentMessage = incidentMessage;
        }
        
        public IncidentMessageBasePayload(IReadOnlyList<UserError> errors)
            : base(errors)
        {
        }
        
        public IncidentMessageBasePayload(UserError error) : base(new [] { error })
        {
        }
    }
}