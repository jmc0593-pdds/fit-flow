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
            //RedisService redisService = new RedisService(_redisDatabase);
            await _redisService.Insert(user, "users", user.Id.ToString());
        }

        //public List<User> GetUsers()
        //{
        //    return _dbContext.User.ToList();
        //}

        //public User? GetUserById(int id)
        //{
        //    return _dbContext.User.FirstOrDefault(user => user.Id == id);
        //}
    }
}
