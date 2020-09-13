using System.Threading.Tasks;
using Coravel.Invocable;
using Coravel.Mailer.Mail.Interfaces;
using Microsoft.AspNetCore.Identity;
using NetCoreStatus.Mailable;

namespace NetCoreStatus.Jobs
{
    public class SendAdminEmail : IInvocable
    {
        private IMailer _mailer;
        private IdentityUser _user;
        public SendAdminEmail(IMailer mailer, IdentityUser user)
        {
            _mailer = mailer;
            _user = user;
        }
        public Task Invoke()
        {
            return _mailer.SendAsync(new AdminStatusMailable(_user));
        }
    }
}