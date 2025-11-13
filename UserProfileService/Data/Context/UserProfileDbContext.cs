using Microsoft.EntityFrameworkCore;

namespace UserProfileService.Data.Context
{
    public class UserProfileDbContext: DbContext
    {
        public UserProfileDbContext(DbContextOptions<UserProfileDbContext> dbContextOptions) :base(dbContextOptions)
        {
            
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(UserProfileDbContext).Assembly);
        }

       

    }
}
