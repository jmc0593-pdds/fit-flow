namespace fit_flow_users.WebApi.DTOs
{
    public record class GoalSet
    {
        public int userId { get; set; }
        public string Goal { get; set; }
    }
}
