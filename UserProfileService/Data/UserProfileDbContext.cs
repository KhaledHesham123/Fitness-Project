using Microsoft.EntityFrameworkCore;
using UserProfileService.Data.Configration;

namespace UserProfileService.Data
{
    public class UserProfileDbContext : DbContext
    {
        public UserProfileDbContext(DbContextOptions<UserProfileDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)  
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(UserProfileDbContext).Assembly);
        }
    }
}
