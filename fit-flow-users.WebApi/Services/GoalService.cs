using fit_flow_users.WebApi.DTOs;
using Microsoft.Extensions.Primitives;
using StackExchange.Redis;
using System.Text.Json;

namespace fit_flow_users.WebApi.Services
{
    public class GoalService
    {
        private readonly string _routinesUrl;
        private readonly RedisService _redisService;

        public GoalService(IConfiguration configurations, RedisService redisService)
        {
            _routinesUrl = configurations.GetValue<string>("RoutinesUrl");
            _redisService = redisService;
        }
            

        public async Task<List<string>> GetGoalsAsync()
        {
            using (var client = new HttpClient())
            {
                List<string> listOfGoals = [];
                Uri routinesUri = new Uri(_routinesUrl);
                Uri endpointUri = new Uri(routinesUri, "api/v1/routines/goals");
                try
                {
                    HttpResponseMessage response = await client.GetAsync(endpointUri);
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
            await _redisService.InsertInListAsync("goal-set", goalSet);
            Console.WriteLine("Goal created");
        }

        public async Task<RoutineRecomended> GetRoutineRecommendedAsync()
        {
            string eventName = "routine-recommended";
            var redisValue = await _redisService.ReadQueue(eventName);
            var routineRecomended = JsonSerializer.Deserialize<RoutineRecomended>(redisValue);
            return routineRecomended;
        }

        public async Task<Routine> GetRoutineAsync(RoutineRecomended routineRecommended)
        {
            Routine? routine = new();
            Uri routinesUri = new(_routinesUrl);
            Uri endpointUri = new(routinesUri, "api/v1/routines/by-goal");
            using (var client = new HttpClient())
            {
                try
                {
                    HttpResponseMessage response = await client.GetAsync(endpointUri);
                    response.EnsureSuccessStatusCode();
                    if (response != null)
                    {
                        string jsonContent = await response.Content.ReadAsStringAsync();
                        var workoutData = JsonSerializer.Deserialize<WorkoutData>(jsonContent);
                        routine = workoutData?.Goal.Routines.Where(routine => routine.Id == routineRecommended.routine_id).FirstOrDefault();
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                }
                return routine;
            }
        }
    }
}
