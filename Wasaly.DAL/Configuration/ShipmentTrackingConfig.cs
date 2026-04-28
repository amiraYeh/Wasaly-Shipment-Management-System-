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
    public class ShipmentTrackingConfig :IEntityTypeConfiguration<ShipmentTracking>
    {
        public void Configure(EntityTypeBuilder<ShipmentTracking> builder)
        {
            builder.Property(x => x.Status)
                   .IsRequired();

            builder.Property(x => x.TimeStamp)
                   .HasDefaultValueSql("GETDATE()");

            builder.Property(x => x.Status)
                  .HasConversion<string>()
                  .IsRequired();

            builder.ToTable(t =>
                t.HasCheckConstraint("CK_Shipment_Status",
                "[Status] IN ('Created','Accepted','PickedUp','Delivered')")
            );

            builder.HasOne(x => x.Shipment)
                .WithMany(x => x.ShipmentTrackings)
                .HasForeignKey(x => x.ShipmentId)
                .OnDelete(DeleteBehavior.Cascade);

        }
    }
}
