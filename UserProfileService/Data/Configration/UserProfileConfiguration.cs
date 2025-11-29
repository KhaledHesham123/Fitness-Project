using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UserProfileService.Entites;

namespace UserProfileService.Data.Configration
{
    public class UserProfileConfiguration : IEntityTypeConfiguration<UserProfile>
    {
        public void Configure(EntityTypeBuilder<UserProfile> builder)
        {
            builder.HasMany(x => x.ProgressHistory)
                   .WithOne()
                   .HasForeignKey(p => p.UserId)
                   .OnDelete(DeleteBehavior.Cascade);


            builder.Property(x => x.Gender)
                    .HasConversion<string>();

            builder.Property(x => x.FitnessGoal)
                    .HasConversion<string>();

            builder.Property(e => e.Height) 
                    .HasPrecision(5, 2);

            builder.Property(e => e.Weight)
                    .HasPrecision(5, 2);
        }
    }
}
