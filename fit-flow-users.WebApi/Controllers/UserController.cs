using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using fit_flow_users.WebApi.Models;
using fit_flow_users.WebApi.Services;
using fit_flow_users.WebApi.Data;
using fit_flow_users.WebApi.DTOs;
using fit_flow_users.WebApi.Mapping;
using StackExchange.Redis;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

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
        private IConnectionMultiplexer _connectionMultiplexer;
        public UserController(IConfiguration configurations, IConnectionMultiplexer connectionMultiplexer, UserService userService, GoalService goalService)
        {
            _configuration = configurations;
            _redisDatabase = connectionMultiplexer.GetDatabase();
            _userService = userService;
            _goalService = goalService;
            _connectionMultiplexer = connectionMultiplexer;
        }

        [HttpPost]
        public async Task<ActionResult> CreateUser([FromBody] CreateUserDto newUser)
        {
            try
            {
                List<string> goals = await _goalService.GetGoalsAsync();
                if (!goals.Contains(newUser.Goal))
                    return NotFound("Goal specified is not present in our internal List");

                User createdUser = newUser.ConvertToEntity();
                await _userService.CreateUser(createdUser);
                await _goalService.SetGoalAsync(createdUser.Id, createdUser.Goal);
                object? routineRecomended = await _goalService.GetRoutineRecommendedAsync();
                if (routineRecomended != null)
                    createdUser.WorkoutData = await _goalService.GetRoutineAsync(routineRecomended);

                return CreatedAtAction(nameof(CreateUser), new { id = createdUser.Id }, createdUser);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.ToString());
            }
        }

        [HttpGet]
        public async Task<ActionResult> GetUsers()
        {
            List<GetUserDto> users = [];
            await _userService.GetUsers();
            return Ok(users);
        }
    }    
}
