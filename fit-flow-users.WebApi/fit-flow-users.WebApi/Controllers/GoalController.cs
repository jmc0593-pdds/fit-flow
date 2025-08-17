using Microsoft.AspNetCore.Mvc;
using fit_flow_users.WebApi.Models;
using fit_flow_users.WebApi.Services;
using fit_flow_users.WebApi.Data;
using fit_flow_users.WebApi.DTOs;
using fit_flow_users.WebApi.Mapping;

namespace fit_flow_users.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GoalController : ControllerBase
    {
        private readonly UserContext _dbContext;
        private readonly IConfiguration _configuration;

        public GoalController(UserContext dbContext, IConfiguration configuration)
        {
            _dbContext = dbContext;
            _configuration = configuration;
        }

        //[HttpGet]
        //public ActionResult<GoalSet> GoalSet()
        //{
        //    GoalService goalService = new GoalService();
        //    await goalService.SetGoal();
        //}
    }
}
