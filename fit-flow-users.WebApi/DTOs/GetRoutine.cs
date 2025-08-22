using System.Text.Json.Serialization;

namespace fit_flow_users.WebApi.DTOs
{
    public class WorkoutData
    {
        [JsonPropertyName("Strength")]
        public Goal Goal { get; set; }
    }

    public class Goal
    {
        [JsonPropertyName("routines")]
        public List<Routine> Routines { get; set; } = new List<Routine>();
    }

    public class Routine
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonPropertyName("exercises")]
        public List<Exercise> Exercises { get; set; } = new List<Exercise>();
    }

    public class Exercise
    {
        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("intensity")]
        public Intensity Intensity { get; set; }
    }

    public class Intensity
    {
        [JsonPropertyName("Set")]
        public SetDetails Set { get; set; }
    }

    public class SetDetails
    {
        [JsonPropertyName("sets")]
        public int Sets { get; set; }

        [JsonPropertyName("reps")]
        public int Reps { get; set; }

        [JsonPropertyName("load")]
        public string Load { get; set; }
    }
}
