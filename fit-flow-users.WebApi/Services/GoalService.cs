using fit_flow_users.WebApi.Data;
using fit_flow_users.WebApi.DTOs;
using StackExchange.Redis;

namespace fit_flow_users.WebApi.Services
{
    public class GoalService
    {
        private readonly IConfiguration _configuration;
        private readonly string? _baseUrl;
        private readonly IDatabase _redisDatabase;
        private readonly RedisService _redisService;

        public GoalService(IConfiguration configurations, IConnectionMultiplexer connectionMultiplexer, RedisService redisService)
        {
            _configuration = configurations;
            _baseUrl = configurations.GetValue<string>("RoutinesUrl");
            _redisDatabase = connectionMultiplexer.GetDatabase();
            _redisService = redisService;
        }
            

        public async Task<List<string>> GetGoalsAsync()
        {
            using (var client = new HttpClient())
            {
                List<string> listOfGoals = [];
                string baseUrl = $"{_baseUrl}/api/v1/routines/goals".Replace("////", "//");
                try
                {
                    HttpResponseMessage response = await client.GetAsync(baseUrl);
                    response.EnsureSuccessStatusCode();
                    if (response != null)
                    {
                        string jsonContent = await response.Content.ReadAsStringAsync();
                        var goals = System.Text.Json.JsonSerializer.Deserialize<GetGoals>(jsonContent);
                        goals?.Goals.ForEach(goal => listOfGoals.Add(goal));
                    }
                }
                catch(Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                }
                return listOfGoals;
            }
        }

        public async Task SetGoalAsync(Guid userId, string goal)
        {
            GoalSet goalSet = new GoalSet(userId, goal);
            await _redisService.InsertAsync(goalSet, "goal-set", goalSet.userId.ToString());
            Console.WriteLine("Goal created");
        }
    }
}
