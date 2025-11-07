using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WorkoutCatalogService.Shared.Entites;

namespace WorkoutCatalogService.Data.Configration
{
    public class WorkoutPlanConfigrations : IEntityTypeConfiguration<WorkoutPlan>
    {
        public void Configure(EntityTypeBuilder<WorkoutPlan> builder)
        {
            builder.Property(x => x.DifficultyLevel).
               HasConversion(DifficultyLevel => DifficultyLevel.ToString()
               , dbValue => (DifficultyLevel)Enum.Parse(typeof(DifficultyLevel), dbValue));

            builder.HasMany(x=>x.Exercises).WithOne(x=>x.WorkoutPlan).HasForeignKey(x=>x.WorkoutPlanId).OnDelete(DeleteBehavior.Cascade);
        }
    }
}
