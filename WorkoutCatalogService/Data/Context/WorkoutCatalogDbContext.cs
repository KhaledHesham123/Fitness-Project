using Microsoft.EntityFrameworkCore;

namespace WorkoutCatalogService.Data.Context
{
    public class WorkoutCatalogDbContext: DbContext
    {
        public WorkoutCatalogDbContext(DbContextOptions<WorkoutCatalogDbContext> dbContextOptions) :base(dbContextOptions)
        {
            
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(WorkoutCatalogDbContext).Assembly);
        }

        public DbSet<Shared.Entites.category> Category { get; set; }
        public DbSet<Shared.Entites.SubCategory> SubCategory { get; set; }

        public DbSet<Shared.Entites.Exercise> Exercises { get; set; }
        public DbSet<Shared.Entites.WorkoutPlan> WorkoutPlans { get; set; }
        public DbSet<Shared.Entites.WorkoutExercise> WorkoutExercise { get; set; }

    }
}
