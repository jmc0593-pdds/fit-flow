using Microsoft.AspNetCore.Mvc;

namespace fit_flow_users.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class Status : Controller
    {
        [HttpGet]
        public ActionResult GetStatus()
        {
            return Ok($"Api is up and running. UTC time: {DateTime.UtcNow}");
        }
    }
}
