using System.Text.Json.Serialization;

namespace fit_flow_users.WebApi.DTOs
{
    public record class GetGoals
    {
        [JsonPropertyName("goals")]
        public List<string> Goals { get; set; }
    }

}
