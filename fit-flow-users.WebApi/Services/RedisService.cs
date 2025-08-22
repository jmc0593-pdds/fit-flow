using fit_flow_users.WebApi.DTOs;
using StackExchange.Redis;
using fit_flow_users.WebApi.Models;

using System.Text.Json;
using Microsoft.Extensions.ObjectPool;

namespace fit_flow_users.WebApi.Services
{
    public class RedisService
    {
        private IDatabase _redisDatabase;
        private ISubscriber _subscriber;


        public RedisService(IConnectionMultiplexer connectionMultiplexer)
        {
            _redisDatabase = connectionMultiplexer.GetDatabase();
            _subscriber = connectionMultiplexer.GetSubscriber();
        }

        public async Task InsertKeyValueAsync(object insertedValue, string prefix, string id)
        {
            string jsonString = JsonSerializer.Serialize(insertedValue);
            try
            {
                bool wasInserted = await _redisDatabase.StringSetAsync($"{prefix}:{id}", jsonString);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        public async Task InsertInListAsync(string listName, object insertedValue)
        {
            string jsonString = JsonSerializer.Serialize(insertedValue);
            try
            {   
                await _redisDatabase.ListRightPushAsync(listName, jsonString);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        public async Task<RedisValue> ReadQueue(string queue)
        {
            RedisValue queueValue = new RedisValue();
            while (true)
            {
                var result = await _redisDatabase.ListRightPopAsync(queue);
                if (!result.IsNullOrEmpty)
                {
                    queueValue = result;
                    break;
                }
                await Task.Delay(200);
            }
            return queueValue;
        }

        public async Task GetAsync(string key)
        {
            var value = await _redisDatabase.StringGetAsync(key);
            int x = 0;
        }
    }
}
