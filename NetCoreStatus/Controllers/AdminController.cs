using Coravel.Mailer.Mail.Interfaces;
using Coravel.Queuing.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NetCoreStatus.Data;

namespace NetCoreStatus.Controllers
{
    public class AdminController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<AdminController> _logger;
        private readonly IQueue _queue;
        private readonly IMailer _mailer;
        
        public AdminController(ILogger<AdminController> logger, ApplicationDbContext context, IQueue queue, IMailer mailer)
        {
            _context = context;
            _logger = logger;
            _queue = queue;
            _mailer = mailer;
        }
        // GET
        [Authorize]
        public IActionResult Index()
        {
            return View();
        }
    }
}