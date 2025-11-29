using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UserProfileService.Entites;

namespace UserProfileService.Data.Configration
{
    public class UserProgressConfiguration : IEntityTypeConfiguration<UserProgress>
    {
        public void Configure(EntityTypeBuilder<UserProgress> builder)
        {
            builder.Property(x => x.CurrentWeight)
                   .HasPrecision(5, 2);
        }
    }
}
