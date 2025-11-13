using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UserProfileService.Shared.Entites;

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


            builder.Property(x => x.Gender).
              HasConversion(DifficultyLevel => DifficultyLevel.ToString()
              , dbValue => (Gender)Enum.Parse(typeof(Gender), dbValue));

            builder.Property(x => x.FitnessGoal).
              HasConversion(DifficultyLevel => DifficultyLevel.ToString()
              , dbValue => (FitnessGoal)Enum.Parse(typeof(FitnessGoal), dbValue));

            builder.Property(x => x.ActivityLevel).
              HasConversion(DifficultyLevel => DifficultyLevel.ToString()
              , dbValue => (ActivityLevel)Enum.Parse(typeof(ActivityLevel), dbValue));


        }
    }
}
