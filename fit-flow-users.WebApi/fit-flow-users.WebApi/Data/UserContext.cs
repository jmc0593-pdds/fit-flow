using fit_flow_users.WebApi.Models;
using Microsoft.EntityFrameworkCore;

namespace fit_flow_users.WebApi.Data
{
    public class UserContext: DbContext
    {
        public UserContext(DbContextOptions<UserContext> options) : base(options) { }

        public DbSet<User> User { get; set; }
}
}
