using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UserProfileService.Shared.Entites;

namespace UserProfileService.Data.Configration
{
    public class UserProgressConfiguration : IEntityTypeConfiguration<UserProgress>
    {
        public void Configure(EntityTypeBuilder<UserProgress> builder)
        {
            builder.Property(x => x.CurrentWeight)
                 .HasColumnType("decimal(10,2)");
        }
    }
}
