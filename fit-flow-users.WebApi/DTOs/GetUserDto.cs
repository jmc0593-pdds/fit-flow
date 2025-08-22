namespace fit_flow_users.WebApi.DTOs
{
    public record class GetUserDto(
        Guid Id,
        string Name,
        string Email,
        string Goal,
        WorkoutData WorkoutData
        );
}
