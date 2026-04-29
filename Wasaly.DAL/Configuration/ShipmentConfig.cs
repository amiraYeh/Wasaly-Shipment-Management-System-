using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using Wasaly.DAL.Enums;
using Wasaly.DAL.Models;

namespace Wasaly.DAL.Configuration
{
    public class ShipmentConfig : IEntityTypeConfiguration<Shipment>
    {
        public void Configure(EntityTypeBuilder<Shipment> builder)
        {

            builder.Property(x => x.TrackingNumber)
              .IsRequired();

            builder.HasIndex(x => x.TrackingNumber)
                   .IsUnique();

            builder.Property(x => x.DeliveredAt)
              .IsRequired();


            builder.Property(x => x.Description)
                   .HasMaxLength(300);

            builder.Property(x => x.Weight)
                   .IsRequired();

            builder.Property(x => x.Price)
                   .HasColumnType("decimal(10,2)");

            builder.Property(x => x.Status)
                   .IsRequired();

            builder.Property(x => x.CreatedAt)
                   .HasDefaultValueSql("GETDATE()");


            builder.Property(x => x.Status)
                   .HasConversion<string>()
                     .HasDefaultValue(ShipmentStatus.Created)
                   .IsRequired();

            builder.ToTable(t =>
                t.HasCheckConstraint("CK_Shipment_Status",
                "[Status] IN ('Created','Accepted','PickedUp','Delivered')")
            );


            builder.HasOne(x => x.Merchant)
                   .WithMany(x=>x.shipments)
                   .HasForeignKey(x => x.MerchantId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.CourierAssignment)
                   .WithMany()
                   .HasForeignKey(x => x.CourierAssignmentId)
                   .OnDelete(DeleteBehavior.SetNull);

            builder.HasOne(x => x.DeliveryOTP)
                  .WithMany()
                  .HasForeignKey(x => x.DeliveryOTPId)
                  .OnDelete(DeleteBehavior.Restrict);

            //builder.HasOne(x => x.ShipmentTracking)
            //      .WithMany()
            //      .HasForeignKey(x => x.ShipmentTrackingId)
            //      .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(x => x.ShipmentTrackings)
              .WithOne(x => x.Shipment)
              .HasForeignKey(x => x.ShipmentId)
              .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(s => s.PickupLocation)
                .WithMany(l => l.PickupShipments)
                .HasForeignKey(s => s.PickupLocationId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(s => s.DropLocation)
                .WithMany(l => l.DropShipments)
                .HasForeignKey(s => s.DropLocationId)
                .OnDelete(DeleteBehavior.Restrict);
        }

    }
}
