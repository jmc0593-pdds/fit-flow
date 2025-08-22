using fit_flow_users.WebApi.DTOs;

namespace fit_flow_users.WebApi.Models
{
    public record class User()
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Goal { get; set; }
        public DateTime CreatedAt { get; set; }
        public WorkoutData WorkoutData { get; set; }

    }
}
    