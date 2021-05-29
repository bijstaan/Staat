using System;
using System.Threading.Tasks;
using Coravel.Invocable;
using Coravel.Queuing.Interfaces;
using Microsoft.EntityFrameworkCore;
using Staat.Data;
using Staat.Jobs.Checks;
using Staat.Models;

namespace Staat.Jobs
{
    public class CheckForJobs : IInvocable
    {
        private readonly ApplicationDbContext _context;
        private IQueue _queue;
        
        public CheckForJobs(IDbContextFactory<ApplicationDbContext> contextFactory, IQueue queue)
        {
            _context = contextFactory.CreateDbContext();
            _queue = queue;
        }
        
        public async Task Invoke()
        {
            var monitors = _context.Monitor;
            foreach (var monitor in monitors)
            {
                // If the next run time is less than or equal to right now, execute the check
                if (monitor.NextRunTime <= DateTime.UtcNow)
                {
                    switch (monitor.Type)
                    {
                        case MonitorType.HTTP:
                            try
                            {
                                await new HttpCheck(_context).Invoke(monitor);
                            }
                            catch (Exception e)
                            {
                                Console.WriteLine(e.Message);
                            }
                            break;
                        case MonitorType.HTTPS:
                            try
                            {
                                await new HttpsCheck(_context).Invoke(monitor);
                            }
                            catch (Exception e)
                            {
                                Console.WriteLine(e.Message);
                            }
                            break;
                        case MonitorType.ICMP:
                            await new IcmpCheck(_context).Invoke(monitor);
                            break;
                        case MonitorType.TCP:
                            await new TcpCheck(_context).Invoke(monitor);
                            break;
                        case MonitorType.SMTP:
                            await new SmtpCheck(_context).Invoke(monitor);
                            break;
                    }
                }
            }
        }
    }
}