using fit_flow_users.WebApi.DTOs;
using StackExchange.Redis;
using fit_flow_users.WebApi.Models;

using System.Text.Json;

namespace fit_flow_users.WebApi.Services
{
    public class RedisService
    {
        private IDatabase _redisDatabase;

        public RedisService(IConnectionMultiplexer connectionMultiplexer)
        {
            _redisDatabase = connectionMultiplexer.GetDatabase();
        }

        public async Task InsertAsync(object insertedValue, string prefix, string id)
        {
            string jsonString = JsonSerializer.Serialize(insertedValue);
            try
            {
                await _redisDatabase.StringSetAsync($"{prefix}:{id}", jsonString);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        public async Task GetRoutineRecommendedAsync()
        {

        }
    }
}
