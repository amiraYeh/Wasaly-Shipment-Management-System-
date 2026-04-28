using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using Wasaly.DAL.Models;

namespace Wasaly.DAL.Configuration
{
    internal class ShipmentConfig : IEntityTypeConfiguration<Shipment>
    {
        public void Configure(EntityTypeBuilder<Shipment> builder)
        {

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
