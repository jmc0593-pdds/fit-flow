using fit_flow_users.WebApi.DTOs;
using fit_flow_users.WebApi.Models;
using System.Xml.Linq;
namespace fit_flow_users.WebApi.Mapping
{
    public static class UserMapping
    {
        public static User ConvertToEntity(this CreateUserDto user)
        {
            return new User()
            {
                Name = user.Name,
                Email = user.Email,
                Goal = user.Goal
            };
        }

        public static CreateUserDto ConvertToCreateUserDto(this User user)
        {
            return new CreateUserDto(
                user.Name,
                user.Email,
                user.Goal
                );
        }

        public static GetUserDto ConvertToGetUserDto(this User user)
        {
            return new GetUserDto(
                user.Id,
                user.Name,
                user.Email,
                user.Goal
                );
        }
    }
}
