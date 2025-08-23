using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using fit_flow_users.WebApi.Models;
using fit_flow_users.WebApi.Services;
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
        private IConnectionMultiplexer _connectionMultiplexer;
        public UserController(IConfiguration configurations, IConnectionMultiplexer connectionMultiplexer, UserService userService, GoalService goalService)
        {
            _configuration = configurations;
            _redisDatabase = connectionMultiplexer.GetDatabase();
            _userService = userService;
            _goalService = goalService;
            _connectionMultiplexer = connectionMultiplexer;
        }

        [HttpGet]
        public async Task<ActionResult> GetUsers() => Ok(await _userService.GetUsers("users:*"));

        [HttpGet("{id}")]
        public async Task<ActionResult> GetUser(string id)
        {
            List<User> users = await _userService.GetUsers($"users:{id}");
            if (users.Any())
                return Ok(users);

            return NotFound($"User with the id {id} not found.");
        } 

        [HttpPost]
        public async Task<ActionResult> CreateUser([FromBody] CreateUserDto newUser)
        {
            try
            {
                List<string> goals = await _goalService.GetGoalsAsync();
                if (!goals.Contains(newUser.Goal))
                    return NotFound($"Goal {newUser.Goal} is not present in our internal List");

                User createdUser = newUser.ConvertToEntity();
                createdUser.Id = Guid.NewGuid();
                await _goalService.SetGoalAsync(createdUser.Id, createdUser.Goal);
                RoutineRecomended? routineRecomended = await _goalService.GetRoutineRecommendedAsync();
                if (routineRecomended != null)
                    createdUser.Routine = await _goalService.GetRoutineAsync(routineRecomended);
                
                await _userService.CreateUser(createdUser);
                return CreatedAtAction(nameof(CreateUser), new { id = createdUser.Id }, createdUser);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.ToString());
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteUser(string id)
        {
            string pattern = $"users:{id}";
            List<User> users = await _userService.GetUsers(pattern);
            if (!users.Any())
                return NotFound($"Not found the id {id} to be deleted.");

            await _userService.DeleteUserAsync(pattern);
            return NoContent();
        }
    }    
}
