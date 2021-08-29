using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Staat.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class SubscriberController : ControllerBase
    {
        [HttpPost]
        public async Task<IActionResult> AddSubscriber()
        {
            return Ok();
        }

        [HttpGet("{activationString}")]
        public async Task<IActionResult> GetActivation(string activationString)
        {
            return Ok();
        }

        [HttpPost("{activationString}")]
        public async Task<IActionResult> PostActivation(string activationString)
        {
            return Ok();
        }
    }
}