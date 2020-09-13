using Coravel.Mailer.Mail;
using Microsoft.AspNetCore.Identity;

namespace NetCoreStatus.Mailable
{
    public class AdminStatusMailable : Mailable<IdentityUser>
    {
        private IdentityUser _user;

        public AdminStatusMailable(IdentityUser user)
        {
            _user = user;
        }

        public override void Build()
        {
            this.To(this._user).Subject("Service Status").View("~/Views/Mail/AdminStatus.cshtml", this._user);
        }
    }
}