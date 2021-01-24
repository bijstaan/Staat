using System.Threading.Tasks;
using Coravel.Invocable;
using Coravel.Mailer.Mail.Interfaces;
using Microsoft.AspNetCore.Identity;
using NetCoreStatus.Mailable;
using NetCoreStatus.Models;

namespace NetCoreStatus.Jobs
{
    public class SendAdminStatusEmail : IInvocable, IInvocableWithPayload<Service>
    {
        public Service Payload { get; set; }
        private IMailer _mailer;
        private IdentityUser _user;
        private Service _service;
        public SendAdminStatusEmail(IMailer mailer, IdentityUser user, Service service)
        {
            _mailer = mailer;
            _user = user;
            _service = service;
        }

        public Task Invoke()
        {
            return _mailer.SendAsync(new AdminStatusMailable(_user, _service));
        }
    }
}