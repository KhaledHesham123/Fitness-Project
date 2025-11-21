using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WorkoutCatalogService.Shared.Entites;

namespace WorkoutCatalogService.Data.Configration
{
    public class SubCategoryConfigrations : IEntityTypeConfiguration<SubCategory>
    {
        public void Configure(EntityTypeBuilder<SubCategory> builder)
        {
            builder.HasMany(x => x.Workout).
                WithOne(x => x.SubCategory).HasForeignKey(x => x.SubCategoryId)
                .OnDelete(DeleteBehavior.Cascade);

        }
    }
}
