using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UserTrainingTrackingService.Domain.Entities;

namespace UserTrainingTrackingService.Infrastructure.Persistence.Data.Configurations
{
    public class UserWorkoutSessionConfiguration : IEntityTypeConfiguration<UserWorkoutSession>
    {
        public void Configure(EntityTypeBuilder<UserWorkoutSession> builder)
        {
            builder.HasKey(ws => ws.Id);
            builder.Property(ws => ws.UserId).IsRequired();
            builder.Property(ws => ws.WorkoutPlanId).IsRequired();
            builder.Property(ws => ws.StartTime).IsRequired();
            builder.HasMany(ws => ws.CompletedExercises)
                   .WithOne(e => e.UserWorkoutSession)
                   .HasForeignKey(e => e.UserWorkoutSessionId);
            builder.Property(ws => ws.Status)
                   .HasConversion<int>();
            builder.HasIndex(ws => ws.UserId);
            builder.HasIndex(ws => ws.WorkoutPlanId);
        }
    }
}
