using Microsoft.EntityFrameworkCore;
using UserTrainingTrackingService.Domain.Entities;

namespace UserTrainingTrackingService.Infrastructure.Persistence.Data
{
    public class UserTrainingTrackingDbContext : DbContext
    {
        public UserTrainingTrackingDbContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<UserWorkoutSession> UserWorkoutSessions { get; set; }
        public DbSet<WorkoutExerciseCompletion> WorkoutExerciseCompletions { get; set; }

        override protected void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(AssemplyReference).Assembly);
            base.OnModelCreating(modelBuilder);
                   
        }
    }
}
