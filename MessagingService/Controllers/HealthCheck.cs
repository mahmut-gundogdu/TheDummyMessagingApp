 
using MessagingService.Core;
using Microsoft.AspNetCore.Mvc;

namespace MessagingService.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class HealthCheckController : ControllerBase   {
        // GET
        public IActionResult Index()
        {
            return Ok("It is working");
        }

        [HttpPost]
        [Authorize]
        public IActionResult Post()
        {
            return Ok("It Works with authorize");
        }
    }
}