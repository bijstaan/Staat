using System;
using System.Net;
using System.Threading.Tasks;
using Coravel.Invocable;
using Microsoft.EntityFrameworkCore;
using Staat.Data;
using Staat.Models;

namespace Staat.Jobs
{
    public class HttpCheck : IInvocable
    {
        public Monitor _monitor;
        public Service _service;
        public HttpCheck(Monitor monitor, Service service)
        {
            _monitor = monitor;
            _service = service;
        }
        
        public async Task Invoke()
        {
            HttpWebRequest request = WebRequest.Create(_service.Url) as HttpWebRequest;
            if (request != null)
                request.UserAgent =
                    "Mozilla/5.0 (compatible: NetCoreStatus/1.0: +https://github.com/tankerkiller125/NetCoreStatus)";
            bool serviceAvailable;
            string failureReason = "";
            try
            {
                if (request != null) request.GetResponse();
                serviceAvailable = true;
            }
            catch (Exception e)
            {
                serviceAvailable = false;
                failureReason = e.Message;
            }
        }
    }
}