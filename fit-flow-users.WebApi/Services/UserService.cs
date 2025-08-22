using fit_flow_users.WebApi.Models;
using fit_flow_users.WebApi.Mapping;
using StackExchange.Redis;

namespace fit_flow_users.WebApi.Services
{
    public class UserService
    {
        private readonly RedisService _redisService;

        public UserService(RedisService redisService)
        {
            _redisService = redisService;
        }
        public async Task CreateUser(User user)
        {
            user.CreatedAt = DateTime.UtcNow;
            user.Id = Guid.NewGuid();
            await _redisService.InsertKeyValueAsync(user, "users", user.Id.ToString());
            Console.WriteLine("User Created");
        }

        public async Task GetUsers()
        {
            await _redisService.GetAsync("user");
        }
    }
}
