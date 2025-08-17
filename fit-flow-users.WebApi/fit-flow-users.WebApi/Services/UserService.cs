using fit_flow_users.WebApi.Data;
using fit_flow_users.WebApi.Models;
using fit_flow_users.WebApi.Mapping;

namespace fit_flow_users.WebApi.Services
{
    public class UserService
    {
        private readonly UserContext _dbContext;
        public UserService(UserContext dbContext)
        {
            _dbContext = dbContext;
        }
        public void CreateUser(User user)
        {
            user.CreatedAt = DateTime.UtcNow;
            _dbContext.User.Add(user);
            _dbContext.SaveChanges();
        }

        public List<User> GetUsers()
        {
            return _dbContext.User.ToList();
        }

        public User? GetUserById(int id)
        {
            return _dbContext.User.FirstOrDefault(user => user.Id == id);
        }
    }
}
