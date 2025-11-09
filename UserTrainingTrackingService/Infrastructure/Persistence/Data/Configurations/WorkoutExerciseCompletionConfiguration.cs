using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UserTrainingTrackingService.Domain.Entities;

namespace UserTrainingTrackingService.Infrastructure.Persistence.Data.Configurations
{
    public class WorkoutExerciseCompletionConfiguration : IEntityTypeConfiguration<WorkoutExerciseCompletion>
    {      
        public void Configure(EntityTypeBuilder<WorkoutExerciseCompletion> builder)
        {
            builder.HasKey(e => e.Id);        
            builder.HasOne(e => e.UserWorkoutSession)
                  .WithMany(ws => ws.CompletedExercises)
                  .HasForeignKey(e => e.UserWorkoutSessionId);

            builder.Property(e => e.UserWorkoutSessionId).IsRequired();
            builder.Property(e => e.ExerciseId).IsRequired();
            builder.Property(e => e.IsCompleted).IsRequired();
            builder.HasIndex(e => e.ExerciseId);
            builder.HasIndex(e => e.UserWorkoutSessionId);
        }             
        
    }
}
