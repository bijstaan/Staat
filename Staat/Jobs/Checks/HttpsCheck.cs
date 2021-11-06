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
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Staat.Data;
using Staat.Models;

namespace Staat.Jobs.Checks
{
    public class HttpsCheck
    {
        public ApplicationDbContext _context;
        public HttpsCheck(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task Invoke(Monitor monitor)
        {
            try
            {
                var first = _context.Service.Where(h => h.Monitors.Contains(monitor)).First();
                var service = _context.Service.First(x => x.Monitors.Contains(monitor));
                Stopwatch sw = new Stopwatch();
                sw.Start();
                HttpWebRequest request = WebRequest.Create(monitor.Host) as HttpWebRequest;
                if (monitor.ValidateSsl != null && (bool) monitor.ValidateSsl)
                {
                    if (request != null) request.ServerCertificateValidationCallback = ValidationCheck;
                }

                if (request != null)
                    request.UserAgent =
                        "Mozilla/5.0 (compatible: NetCoreStatus/1.0: +https://github.com/tankerkiller125/NetCoreStatus)";
                bool serviceAvailable;
                string failureReason = "";
                try
                {
                    await request.GetResponseAsync();
                    serviceAvailable = true;
                }
                catch (Exception e)
                {
                    serviceAvailable = false;
                    failureReason = e.Message;
                }

                sw.Stop();
                if (!serviceAvailable)
                {
                    if (monitor.CurrentIncident == null)
                    {
                        // Get default warning status
                        var status = _context.Status.First(x =>
                            x.Id.Equals(_context.Settings.First(s => s.Key.Equals("status.warning"))));
                        // Set new incident
                        var incident = monitor.CurrentIncident = new Incident()
                        {
                            Title = $"Possible disruption of {service.Name}",
                            StartedAt = DateTime.UtcNow,
                            Service = service,
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
                        if (monitor.CurrentIncident != null)
                        {
                            service.Status = _context.Status.First(x =>
                                x.Id.Equals(_context.Settings.First(s => s.Key.Equals("status.available"))));
                            monitor.CurrentIncident = null;
                        }
                    }
                }
                monitor.MonitorData = new[]
                {
                    new MonitorData
                    {
                        Available = serviceAvailable,
                        Monitor = monitor,
                        FailureReason = failureReason,
                        PingTime = sw.ElapsedMilliseconds
                    }
                };
                monitor.LastRunTime = DateTime.UtcNow;
                var nextRun = DateTime.UtcNow.Add(TimeSpan.Parse(monitor.MonitorCron));
                monitor.NextRunTime = nextRun;
                await _context.SaveChangesAsync();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        private static bool ValidationCheck(object sender, X509Certificate certificate, X509Chain chain,
                SslPolicyErrors sslPolicyErrors)
            {
                if (sslPolicyErrors != SslPolicyErrors.None)
                {
                    return false;
                }
                if (chain.ChainPolicy.VerificationFlags == X509VerificationFlags.NoFlag &&
                    chain.ChainPolicy.RevocationMode == X509RevocationMode.Online)
                {
                    return true;
                }

                X509Chain newChain = new X509Chain();
                X509ChainElementCollection chainElements = chain.ChainElements;
                for (int i = 1; i < chainElements.Count - 1; i++)
                {
                    newChain.ChainPolicy.ExtraStore.Add(chainElements[i].Certificate);
                }

                // Use chainElements[0].Certificate since it's the right cert already
                // in X509Certificate2 form, preventing a cast or the sometimes-dangerous
                // X509Certificate2(X509Certificate) constructor.
                // If the chain build successfully it matches all our policy requests,
                // if it fails, it either failed to build (which is unlikely, since we already had one)
                // or it failed policy (like it's revoked).
                return newChain.Build(chainElements[0].Certificate);
            }
        }
}