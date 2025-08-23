using fit_flow_users.WebApi.Models;
using fit_flow_users.WebApi.Mapping;
using StackExchange.Redis;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;

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
            await _redisService.InsertKeyValueAsync(user, "users", user.Id.ToString());
            Console.WriteLine("User Created");
        }

        public async Task<List<User>> GetUsers(string pattern)
        {
            List<User> userList = [];
            List<RedisValue> redisValueList = await _redisService.GetRedisValuesByPattern(pattern);
            foreach(RedisValue redisValue in redisValueList)
                userList.Add(JsonSerializer.Deserialize<User>(redisValue));

            return userList.OrderBy(user => user.CreatedAt).ToList();
        }
        [HttpDelete]
        public async Task DeleteUserAsync(string pattern)
        {
            await _redisService.DeleteRedisKeysByPattern(pattern);
        }
    }
}
