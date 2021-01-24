using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Coravel.Mailer.Mail.Interfaces;
using Coravel.Queuing.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using NetCoreStatus.Data;
using NetCoreStatus.Models;

namespace NetCoreStatus.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<HomeController> _logger;
        private readonly IQueue _queue;
        private readonly IMailer _mailer;

        public HomeController(ILogger<HomeController> logger, ApplicationDbContext context, IQueue queue, IMailer mailer)
        {
            _context = context;
            _logger = logger;
            _queue = queue;
            _mailer = mailer;
        }

        [ResponseCache(Duration = 60, Location = ResponseCacheLocation.Any, NoStore = false)]
        public async Task<IActionResult> Index()
        {
            var serviceGroups = await _context.ServiceGroups
                .Include(group => group.Services)
                .ThenInclude(service => service.Status)
                .Include(group => group.Services)
                .ThenInclude(service => service.Children)
                .ThenInclude(children => children.Status)
                .ToListAsync();
            return View(serviceGroups);
        }

        public async Task<IActionResult> Privacy()
        {
            //_queue.QueueInvocableWithPayload<SendAdminStatusEmail, Service>(new Service());
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel {RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier});
        }
    }
}