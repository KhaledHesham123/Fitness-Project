

using IdentityService.Shared.Entities;
using Microsoft.EntityFrameworkCore;
using System.Reflection;
using IdentityService.Data.EntitiesConfigurations;

namespace IdentityService.Data.DBContexts
{
    public class IdentityContext : DbContext
    {
        public IdentityContext(DbContextOptions<IdentityContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetAssembly(typeof(UserEntityConfiguration)));

        }
    }
}
