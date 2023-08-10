using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Sipay_Cohort_HW2.Entities;

namespace Sipay_Cohort_HW2.DataAccess.EntityTypeConfigurations
{
    public class OrderConfig : BaseConfig<Order>
    {
        public override void Configure(EntityTypeBuilder<Order> builder)
        {
            base.Configure(builder);
            builder.Property(x => x.UnitPrice).IsRequired();
            builder.Property(x => x.Quantity).IsRequired();
            builder.Property(x => x.Description).IsRequired().HasMaxLength(150);
            builder.HasOne(x => x.user).WithMany(x => x.Orders).HasForeignKey(x => x.UserId);
        }
    }
}
