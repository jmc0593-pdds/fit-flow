using System.ComponentModel.DataAnnotations;

namespace fit_flow_users.WebApi.DTOs
{
    public record class CreateUserDto(
        [Required][StringLength(50)]string Name,
        [Required][EmailAddress]string Email,
        [Required][StringLength(32)]string Goal
        );
}
