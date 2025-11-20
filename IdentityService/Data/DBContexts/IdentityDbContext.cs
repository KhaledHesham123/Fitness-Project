

using Exam_System.Domain.Entities;
using IdentityService.Data.EntitiesConfigurations;
using IdentityService.Shared.Entities;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace IdentityService.Data.DBContexts
{
    public class IdentityDbContext : DbContext
    {
        public IdentityDbContext(DbContextOptions<IdentityDbContext> options) : base(options)
        {
        }
        public DbSet<User> Users => Set<User>();
        public DbSet<Role> Roles => Set<Role>();
        public DbSet<Permission> Permissions => Set<Permission>();

        public DbSet<UserRole> UserRoles => Set<UserRole>();
        public DbSet<RolePermission> RolePermissions => Set<RolePermission>();
        public DbSet<UserPermission> UserPermissions => Set<UserPermission>();
        public DbSet<RefreshToken> RefreshTokens => Set<RefreshToken>();
        public DbSet<UserToken> UserTokens => Set<UserToken>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetAssembly(typeof(UserEntityConfiguration)));

        }
    }
}
