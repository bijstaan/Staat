using System.Collections.Generic;

namespace Staat.Helpers
{
    public abstract class Payload
    {
        public IReadOnlyList<UserError> Errors { get; }
        
        protected Payload(IReadOnlyList<UserError>? errors = null)
        {
            Errors = errors;
        }
    }
}