/*
 * Staat - Staat
 * Copyright (C) 2021 Bijstaan
 *
 * This program is free software: you can redistribute it and/or modify
 * it under the terms of the GNU Affero General Public License as published
 * by the Free Software Foundation, either version 3 of the License, or (at your
 * option) any later version.
 *
 * This program is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU Affero General Public License for more details.
 *
 * You should have received a copy of the GNU Affero General Public License
 * along with this program.  If not, see <https://www.gnu.org/licenses/>.
 */

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