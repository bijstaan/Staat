using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Coravel.Queuing.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using NetCoreStatus.Data;
using NetCoreStatus.Jobs;
using NetCoreStatus.Models;

namespace NetCoreStatus.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<HomeController> _logger;
        private readonly IQueue _queue;

        public HomeController(ILogger<HomeController> logger, ApplicationDbContext context, IQueue queue)
        {
            _context = context;
            _logger = logger;
            _queue = queue;
        }

        [ResponseCache(Duration = 60, Location = ResponseCacheLocation.Any, NoStore = false)]
        public async Task<IActionResult> Index()
        {
            var test = await _context.ServiceGroups
                .Include(group => group.Services).ThenInclude(service => service.Status)
                .ToListAsync();
            return View(test);
        }

        public IActionResult Privacy()
        {
            _queue.QueueInvocable<SendAdminEmail>();
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel {RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier});
        }
    }
}