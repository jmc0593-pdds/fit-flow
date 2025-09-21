using Microsoft.AspNetCore.Http.Timeouts;
using Microsoft.AspNetCore.Mvc;
using Sentry;
namespace fit_flow_users.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class Status : Controller
    {
        [RequestTimeout(milliseconds:3000)]
        [HttpGet]
        public ActionResult GetStatus()
        {
            string message = $"Api is up and running. UTC time: {DateTime.UtcNow}";
            SentrySdk.CaptureMessage(message);
            return Ok(message);
        }
    }
}
