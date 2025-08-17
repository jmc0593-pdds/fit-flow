namespace fit_flow_users.WebApi.DTOs
{
    public record class GetUserDto(
        int Id,
        string Name,
        string Email,
        string Goal
        );
}
