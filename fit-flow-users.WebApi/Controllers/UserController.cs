using Microsoft.AspNetCore.Http;
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
    public class UserController : ControllerBase
    {
        private readonly UserContext _dbContext;
        private readonly IConfiguration _configuration;
        public UserController(UserContext dbContext, IConfiguration configurations)
        {
            _dbContext = dbContext;
            _configuration = configurations;
        }

        [HttpPost]
        public async Task<ActionResult> CreateUser([FromBody] CreateUserDto newUser)
        {
            GoalService goalService = new GoalService(_dbContext, _configuration);
            List<string> goals = await goalService.GetGoals();
            if (!goals.Contains(newUser.Goal))
                return NotFound("Goal specified is not present in our internal List");
            
            UserService userService = new UserService(_dbContext);
            User createdUser = newUser.ConvertToEntity();
            userService.CreateUser(createdUser);

            //await goalService.SetGoal(createdUser.Id, createdUser.Goal);

            return CreatedAtAction(nameof(CreateUser), new { id = createdUser.Id }, createdUser);
        }

        [HttpGet]
        public ActionResult<List<User>> GetUsers()
        {
            UserService userService = new UserService(_dbContext);
            return userService.GetUsers();
        }

        [HttpGet]
        [Route("{id}")]
        public ActionResult<GetUserDto> GetUser(int id)
        {
            UserService userService = new UserService(_dbContext);
            User? user = userService.GetUserById(id);
            if (user != null)
                return user.ConvertToGetUserDto();
            return new NotFoundResult();
        }
    }    
}
