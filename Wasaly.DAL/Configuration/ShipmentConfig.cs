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
              .IsRequired(false);


            builder.Property(x => x.Description)
                   .HasMaxLength(300);

            builder.Property(x => x.Weight)
                   .IsRequired();

            builder.Property(x => x.Price)
                   .HasColumnType("decimal(10,2)");

            builder.Property(x => x.CreatedAt)
                   .HasDefaultValueSql("GETDATE()");

            builder.Property(x => x.RecipientName)
                   .IsRequired()
                   .HasMaxLength(100);

            builder.Property(x => x.RecipientEmail)
                .IsRequired()
                .HasMaxLength(150);

            builder.Property(x => x.RecipientPhone)
                .IsRequired()
                .HasMaxLength(15);


            builder.Property(x => x.Status)
                   .HasConversion<string>()
                     .HasDefaultValue(ShipmentStatus.Created)
                   .IsRequired();

            builder.ToTable(t =>
                t.HasCheckConstraint("CK_Shipment_Status",
                "[Status] IN ('Created','Accepted','PickedUp','Delivered')")
            );


            builder.HasOne(s => s.Merchant)
                   .WithMany(m=>m.shipments)
                   .HasForeignKey(s => s.MerchantId)
                   .OnDelete(DeleteBehavior.Restrict);


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
