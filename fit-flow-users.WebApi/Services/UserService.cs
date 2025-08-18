using fit_flow_users.WebApi.Data;
using fit_flow_users.WebApi.Models;
using fit_flow_users.WebApi.Mapping;
using StackExchange.Redis;

namespace fit_flow_users.WebApi.Services
{
    public class UserService
    {
        private readonly IDatabase _redisDatabase;
        private readonly RedisService _redisService;

        public UserService(IConnectionMultiplexer connectionMultiplexer, RedisService redisService)
        {
            _redisDatabase = connectionMultiplexer.GetDatabase();
            _redisService = redisService;
        }
        public async Task CreateUser(User user)
        {
            user.CreatedAt = DateTime.UtcNow;
            user.Id = Guid.NewGuid();
            await _redisService.Insert(user, "users", user.Id.ToString());
        }
    }
}
