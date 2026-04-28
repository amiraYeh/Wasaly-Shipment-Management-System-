using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wasaly.DAL.Enums;
using Wasaly.DAL.Models;

namespace Wasaly.DAL.Configuration
{
    public class CourierAssignmentConfig : IEntityTypeConfiguration<CourierAssignment>
    {
        public void Configure(EntityTypeBuilder<CourierAssignment> builder)
        {

            builder.Property(x => x.AssignedAt)
                   .HasDefaultValueSql("GETDATE()");


            builder.Property(x => x.Status)
                   .HasConversion<string>()
                     .HasDefaultValue(CourierStatus.Assigned)
                   .IsRequired();

            builder.ToTable(t =>
                t.HasCheckConstraint("CK_Courier_Status",
                "[Status] IN ( 'Assigned', 'Accepted','Rejected')")
            );


            builder.HasOne(x => x.Courier)
                   .WithMany()
                   .HasForeignKey(x => x.CourierId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.Shipment)
                   .WithMany()
                   .HasForeignKey(x => x.ShipmentId)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
