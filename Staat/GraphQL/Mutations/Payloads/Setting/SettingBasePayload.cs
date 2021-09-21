#nullable enable
using System.Collections.Generic;
using Staat.Helpers;

namespace Staat.GraphQL.Mutations.Payloads.Setting
{
    public class SettingBasePayload : Payload
    {
        public Models.Settings? Settings { get;}
        
        public SettingBasePayload(Models.Settings settings)
        {
            Settings = settings;
        }
        
        public SettingBasePayload(IReadOnlyList<UserError> errors)
            : base(errors)
        {
        }
        
        public SettingBasePayload(UserError error) : base(new [] { error })
        {
        }
    }
}