using fit_flow_users.WebApi.Data;
using fit_flow_users.WebApi.DTOs;

namespace fit_flow_users.WebApi.Services
{
    public class GoalService
    {
        private readonly UserContext _dbContext;
        private readonly IConfiguration _configuration;
        private readonly string? _baseUrl;

        public GoalService(UserContext dbContext, IConfiguration configurations)
        {
            _dbContext = dbContext;
            _configuration = configurations;
            _baseUrl = configurations.GetValue<string>("RoutinesUrl");
        }


        public async Task<List<string>> GetGoals()
        {
            using (var client = new HttpClient())
            {
                List<string> listOfGoals = [];
                string baseUrl = $"{_baseUrl}/api/v1/routines/goals".Replace("////", "//");
                HttpResponseMessage response = await client.GetAsync(baseUrl);
                response.EnsureSuccessStatusCode();
                if (response != null)
                {
                    string jsonContent = await response.Content.ReadAsStringAsync();
                    var goals = System.Text.Json.JsonSerializer.Deserialize<GetGoals>(jsonContent);
                    goals?.Goals.ForEach(goal =>  listOfGoals.Add(goal));
                }
                return listOfGoals;
            }
        }

        public async Task SetGoal(int userId, string goal)
        {
            UserService userService = new UserService(_dbContext);
        }
    }
}
