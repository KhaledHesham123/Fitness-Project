using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WorkoutCatalogService.Shared.Entites;

namespace WorkoutCatalogService.Data.Configration
{
    public class WorkoutExerciseConfigrations : IEntityTypeConfiguration<PlanWorkout>
    {
        public void Configure(EntityTypeBuilder<PlanWorkout> builder)
        {
            builder.HasKey(x => new { x.WorkoutPlanId, x.ExerciseId });


            builder.HasOne(x => x.Workout)
                   .WithMany(x => x.PlanWorkout)
                   .HasForeignKey(x => x.ExerciseId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(x => x.WorkoutPlan)
       .WithMany(x => x.PlanWorkout)
       .HasForeignKey(x => x.WorkoutPlanId)
       .OnDelete(DeleteBehavior.Cascade);


        }
    }
}
