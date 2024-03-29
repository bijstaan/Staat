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
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Staat.Data;
using Staat.Data.Models;

namespace Staat.Jobs.Checks
{
    public class HttpCheck
    {
        public ApplicationDbContext _context;

        public HttpCheck(ApplicationDbContext context)
        {
            _context = context;
        }
        
        public async Task Invoke(Monitor monitor)
        {
            var _monitor = monitor;
            var _service = _context.Service.First(x => x.Monitors.Contains(_monitor));
            // Create timing for monitor data
            Stopwatch sw = new Stopwatch();
            sw.Start();
            HttpClientHandler handler = new HttpClientHandler();
            HttpClient client = new HttpClient(handler);
            client.DefaultRequestHeaders.Add("User-Agent", "Staat/1.0 (+https://github.com/Bijstaan/Staat)");
            var response = await client.GetAsync(monitor.Host);
            bool serviceAvailable;
            string failureReason = "";
            if (response.IsSuccessStatusCode)
            {
                serviceAvailable = true;
            }
            else
            {
                serviceAvailable = false;
                failureReason = response.ReasonPhrase;
            }
            sw.Stop();
            if (!serviceAvailable)
            {
                if (_monitor.CurrentIncident == null)
                {
                    // Get default warning status
                    var status = _context.Status.First(x => x.Id.Equals(_context.Settings.First(s => s.Key.Equals("backend.status.warning"))));
                    // Set new incident
                    var incident = _monitor.CurrentIncident = new Incident()
                    {
                        Title = $"Possible disruption of {_service.Name}",
                        StartedAt = DateTime.UtcNow,
                        Service = _service,
                        Description = $"Automated Detection"
                    };
                    // Add message to incident
                    incident.Messages = new List<IncidentMessage>()
                    {
                        new()
                        {
                            Incident = incident,
                            Message = $"Automated Detection: ```{failureReason}```",
                            Status = status
                        }
                    };
                }
                else
                {
                    // If service is OK now clear previous incidents
                    if (_monitor.CurrentIncident != null)
                    {
                        _monitor.Service.Status = _context.Status.First(x => x.Id.Equals(_context.Settings.First(s => s.Key.Equals("backend.status.success"))));
                        _monitor.CurrentIncident = null;
                    }
                }

                monitor.MonitorData = new[]
                {
                    new MonitorData
                    {
                        Available = serviceAvailable,
                        Monitor = _monitor,
                        FailureReason = failureReason,
                        PingTime = sw.ElapsedMilliseconds
                    }
                };
                _monitor.LastRunTime = DateTime.UtcNow;
                var nextRun= DateTime.UtcNow.Add(TimeSpan.Parse(_monitor.MonitorCron));
                _monitor.NextRunTime = nextRun;
                await _context.SaveChangesAsync();
            }
        }
    }
}