using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WorkoutCatalogService.Shared.Entites;

namespace WorkoutCatalogService.Data.Configration
{
    public class ExerciseConfigrations : IEntityTypeConfiguration<Workout>
    {
        public void Configure(EntityTypeBuilder<Workout> builder)
        {
            builder.Property(x=>x.DifficultyLevel).
                HasConversion(DifficultyLevel=> DifficultyLevel.ToString()
                , dbValue => (DifficultyLevel)Enum.Parse(typeof(DifficultyLevel), dbValue));

            builder.Property(x=>x.MuscleGroup).
                HasConversion(DifficultyLevel=> DifficultyLevel.ToString()
                , dbValue => (MuscleGroup)Enum.Parse(typeof(MuscleGroup), dbValue));

            builder.HasMany(x => x.PlanWorkout).WithOne().HasForeignKey(x => x.ExerciseId).OnDelete(DeleteBehavior.Cascade);

        }
    }
}
