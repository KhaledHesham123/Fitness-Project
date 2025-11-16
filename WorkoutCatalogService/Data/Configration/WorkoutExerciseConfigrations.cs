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


           


        }
    }
}
