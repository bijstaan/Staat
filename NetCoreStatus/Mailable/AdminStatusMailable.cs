using Coravel.Mailer.Mail;
using Microsoft.AspNetCore.Identity;
using Microsoft.VisualBasic;
using NetCoreStatus.Models;

namespace NetCoreStatus.Mailable
{
    public class AdminStatusMailable : Mailable<Service>
    {
        private IdentityUser _user;
        private Service _service;

        public AdminStatusMailable(IdentityUser user, Service service)
        {
            _user = user;
            _service = service;
        }

        public override void Build()
        {
            To(_user).Subject("Service Status").View("~/Views/Mail/AdminStatus.cshtml", _service);
        }
    }
}