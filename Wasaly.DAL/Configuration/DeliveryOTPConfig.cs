using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wasaly.DAL.Models;

namespace Wasaly.DAL.Configuration
{
    public class DeliveryOTPConfig:IEntityTypeConfiguration<DeliveryOTP>
    {
        public void Configure(EntityTypeBuilder<DeliveryOTP> builder)
        {
            builder.Property(x => x.OTPCode)
                   .IsRequired();

            builder.ToTable(t =>
                    t.HasCheckConstraint("CK_DeliveryOTP_Code",
                    "[OTPCode] >= 100000 AND [OTPCode] <= 999999"));

            builder.Property(x => x.IsUsed)
                   .HasDefaultValue(false);

            builder.Property(x => x.CreatedAt)
                   .HasDefaultValueSql("GETDATE()");

            builder.Property(x => x.ExpiryTime)
                   .IsRequired();

            builder.Property(x => x.ExpiryTime)
                     .HasDefaultValueSql("DATEADD(MINUTE, 10, GETDATE())"); builder.Property(x => x.ExpiryTime)
                    .HasDefaultValueSql("DATEADD(MINUTE, 10, GETDATE())");
          
            builder.HasOne(x => x.Shipment)
                   .WithOne()
                   .HasForeignKey<DeliveryOTP>(x => x.ShipmentId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasIndex(x => x.ShipmentId);
        }
    }
}
