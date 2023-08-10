using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Sipay_Cohort_HW2.Entities;

namespace Sipay_Cohort_HW2.DataAccess.EntityTypeConfigurations
{
    public class UserConfig : BaseConfig<User>
    {
        public override void Configure(EntityTypeBuilder<User> builder)
        {
            base.Configure(builder);
            builder.Property(x => x.FirstName).IsRequired().HasMaxLength(50);
            builder.Property(x => x.LastName).IsRequired().HasMaxLength(50);
            builder.Property(x => x.Email).IsRequired().HasMaxLength(50);
        }
    }
}
