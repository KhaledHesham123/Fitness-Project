using IdentityService.Shared.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IdentityService.Data.EntitiesConfigurations
{
    public class UserEntityConfiguration : BaseEntityConfiguration<User>
    {
        public override void Configure(EntityTypeBuilder<User> builder)
        {
            base.Configure(builder);
            builder.HasIndex(u => u.Email).IsUnique();


        }
    }
}
