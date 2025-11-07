using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WorkoutCatalogService.Shared.Entites;

namespace WorkoutCatalogService.Data.Configration
{
    public class WorkoutExerciseConfigrations : IEntityTypeConfiguration<WorkoutExercise>
    {
        public void Configure(EntityTypeBuilder<WorkoutExercise> builder)
        {
            builder.HasKey(x => new { x.WorkoutPlanId, x.ExerciseId });


            builder.HasOne(x => x.Exercise)
                   .WithMany(x => x.WorkoutExercises)
                   .HasForeignKey(x => x.ExerciseId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(x => x.WorkoutPlan)
       .WithMany(x => x.Exercises)
       .HasForeignKey(x => x.WorkoutPlanId)
       .OnDelete(DeleteBehavior.Cascade);


        }
    }
}
