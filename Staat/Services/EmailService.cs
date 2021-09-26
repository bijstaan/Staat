using FluentEmail.Core;
using Microsoft.Extensions.Configuration;

namespace Staat.Services
{
    public class EmailService
    {
        public readonly IFluentEmail _fluentEmail;
        public readonly IConfiguration _configuration;
        
        public EmailService(IFluentEmail fluentEmail, IConfiguration configuration)
        {
            _configuration = configuration;
            _fluentEmail = fluentEmail;
        }

        public IFluentEmail Email()
        {
            var url = _configuration.GetSection("App")["Url"];
            //_fluentEmail.Header("List-Unsubscribe", $"<${url}/unsubscribe>");

            return _fluentEmail;
        }
    }
}