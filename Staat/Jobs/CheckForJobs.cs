using System;
using System.Threading.Tasks;
using Coravel.Invocable;
using Microsoft.EntityFrameworkCore;
using Staat.Data;
using Staat.Models;

namespace Staat.Jobs
{
    public class CheckForJobs : IInvocable
    {
        private readonly ApplicationDbContext _context;
        
        public CheckForJobs(IDbContextFactory<ApplicationDbContext> contextFactory)
        {
            _context = contextFactory.CreateDbContext();
        }
        
        public async Task Invoke()
        {
            var monitors = _context.Monitor;
            foreach (var monitor in monitors)
            {
                if (monitor.NextRunTime <= DateTime.UtcNow)
                {
                    if (monitor.Type == MonitorType.HTTP)
                    {
                        
                    }
                    var nextRun= DateTime.UtcNow.Add(TimeSpan.Parse(monitor.MonitorCron));
                    monitor.NextRunTime = nextRun;
                }
            }

            await _context.SaveChangesAsync();
        }
    }
}