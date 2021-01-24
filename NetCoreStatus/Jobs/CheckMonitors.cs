using System;
using System.Linq;
using System.Net;
using System.Net.Security;
using System.Net.Sockets;
using System.Security.Cryptography.X509Certificates;
using System.Threading;
using System.Threading.Tasks;
using Coravel.Invocable;
using Coravel.Queuing.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using NetCoreStatus.Data;
using NetCoreStatus.Models;

namespace NetCoreStatus.Jobs
{
    public class CheckMonitors : IInvocable
    {
        public ApplicationDbContext _context;
        private IQueue _queue;
        public CheckMonitors(ApplicationDbContext context, IQueue queue)
        {
            _context = context;
            _queue = queue;
        }
        public async Task Invoke()
        {
            var monitors = await _context.Monitors
                .Include(m => m.Service)
                .Include(m => m.CurrentIncident)
                .ToListAsync();
            Parallel.ForEach(monitors, (m) =>
            {
                MonitorTestObject serviceAvailable = new MonitorTestObject();
                if (m.Type == "https")
                {
                    serviceAvailable = CheckHttps(m.Host, m.ValidateSsl);
                } else if (m.Type == "http")
                {
                    serviceAvailable = CheckHttp(m.Host);
                } else if (m.Type == "tcp")
                {
                    serviceAvailable = CheckTcp(m.Host, m.Port);
                }
                Console.WriteLine(m.Service.Name + serviceAvailable.ServiceAvailable);
                if (!serviceAvailable.ServiceAvailable)
                {
                    if (m.CurrentIncident == null)
                    {
                        m.CurrentIncident = new Incident()
                        {
                            Title = "Possible Outage of " + m.Service.Name,
                            Service = m.Service,
                            Description = "Automated Detection: " + serviceAvailable.FailureReason,
                        };
                        m.Service.Status = _context.Statuses.First(s => s.IsErrorDefault);
                        _queue.QueueInvocableWithPayload<SendAdminStatusEmail, Service>(m.Service);
                        _context.SaveChangesAsync();
                    }
                }
                else
                {
                    if (m.CurrentIncident != null)
                    {
                        m.Service.Status = _context.Statuses.First(s => s.IsOperationalDefault);
                        m.CurrentIncident = null;
                        _context.SaveChangesAsync();
                    }
                }
            });
        }

        private MonitorTestObject CheckTcp(string host, int port)
        {
            var ipAddresses = Dns.GetHostAddresses(host);
            bool serviceAvailable = false;
            string failureReason = "";
            foreach (var ipAddress in ipAddresses)
            {
                try
                {
                    TcpListener tcpListener = new TcpListener(ipAddress, port);
                    tcpListener.Start();
                    serviceAvailable = true;
                }
                catch (SocketException e)
                {
                    serviceAvailable = false;
                    failureReason = e.Message;
                }
            }

            return new MonitorTestObject()
            {
                ServiceAvailable = serviceAvailable,
                FailureReason = failureReason
            };
        }

        private MonitorTestObject CheckHttps(string host, bool checkValidCerts)
        {
            // Check SSL Cert is valid all the way through the chain
            static bool ValidationCheck(object sender, X509Certificate certificate, X509Chain chain,
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

            HttpWebRequest request = WebRequest.Create(host) as HttpWebRequest;
            if (checkValidCerts)
            {
                request.ServerCertificateValidationCallback = ValidationCheck;
            }
            request.UserAgent =
                "Mozilla/5.0 (compatible: NetCoreStatus/1.0: +https://github.com/tankerkiller125/NetCoreStatus)";
            bool serviceAvailable;
            string failureReason = "";
            try
            {
                request.GetResponse();
                serviceAvailable = true;
            }
            catch (Exception e)
            {
                serviceAvailable = false;
                failureReason = e.Message;
            }

            return new MonitorTestObject()
            {
                ServiceAvailable = serviceAvailable,
                FailureReason = failureReason
            };
        }

        public MonitorTestObject CheckHttp(string host)
        {
            HttpWebRequest request = WebRequest.Create(host) as HttpWebRequest;
            request.UserAgent =
                "Mozilla/5.0 (compatible: NetCoreStatus/1.0: +https://github.com/tankerkiller125/NetCoreStatus)";
            bool serviceAvailable;
            string failureReason = "";
            try
            {
                request.GetResponse();
                serviceAvailable = true;
            }
            catch (Exception e)
            {
                serviceAvailable = false;
                failureReason = e.Message;
            }
            return new MonitorTestObject()
            {
                ServiceAvailable = serviceAvailable,
                FailureReason = failureReason
            };
        }

        public class MonitorTestObject
        {
            public bool ServiceAvailable { get; set; }
            public string FailureReason { get; set; }
        }
    }
}