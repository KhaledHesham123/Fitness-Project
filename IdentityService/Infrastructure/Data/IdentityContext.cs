

using IdentityService.Domain.Entities;
using IdentityService.Infrastructure.EntitiesConfigurations;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace IdentityService.Infrastructure.Data
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
