using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using fit_flow_users.WebApi.Models;
using fit_flow_users.WebApi.Services;
using fit_flow_users.WebApi.Data;
using fit_flow_users.WebApi.DTOs;
using fit_flow_users.WebApi.Mapping;
using StackExchange.Redis;

namespace fit_flow_users.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private IDatabase _redisDatabase;
        private UserService _userService;
        private GoalService _goalService;

        public UserController(IConfiguration configurations, IConnectionMultiplexer connectionMultiplexer, UserService userService, GoalService goalService)
        {
            _configuration = configurations;
            _redisDatabase = connectionMultiplexer.GetDatabase();
            _userService = userService;
            _goalService = goalService;
            /*ConnectionMultiplexer connection = ConnectionMultiplexer.Connect(_configuration.GetConnectionString("Redis"));
            _redisDatabase = connection.GetDatabase();*/
        }

        [HttpPost]
        public async Task<ActionResult> CreateUser([FromBody] CreateUserDto newUser)
        {
            //GoalService goalService = new GoalService(_configuration, _redisDatabase);
            List<string> goals = await _goalService.GetGoals();
            if (!goals.Contains(newUser.Goal))
                return NotFound("Goal specified is not present in our internal List");
            
            //UserService userService = new UserService(_redisDatabase);
            User createdUser = newUser.ConvertToEntity();
            await _userService.CreateUser(createdUser);

            await _goalService.SetGoal(createdUser.Id, createdUser.Goal);

            return CreatedAtAction(nameof(CreateUser), new { id = createdUser.Id }, createdUser);
        }


        //[HttpGet]
        //public ActionResult<List<User>> GetUsers()
        //{
        //    UserService userService = new UserService(_dbContext);
        //    return userService.GetUsers();
        //}

        //[HttpGet]
        //[Route("{id}")]
        //public ActionResult<GetUserDto> GetUser(int id)
        //{
        //    UserService userService = new UserService(_dbContext);
        //    User? user = userService.GetUserById(id);
        //    if (user != null)
        //        return user.ConvertToGetUserDto();
        //    return new NotFoundResult();
        //}
    }    
}
