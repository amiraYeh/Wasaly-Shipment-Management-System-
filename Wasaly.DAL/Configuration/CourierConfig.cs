using Day9Demo.Models;
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
    // Remove or rename this class if another CourierConfig exists in this namespace.
    // If you need both, use a different class name for one of them.
    public class CourierConfig : IEntityTypeConfiguration<Courier>

    {
        public void Configure(EntityTypeBuilder<Courier> builder)
        {
            builder.Property(c => c.NationalIdImagePath)
                .IsRequired(false)
                .HasMaxLength(200);

            builder.Property(c => c.DrivingLicenseImagePath)
                .IsRequired(false)
                .HasMaxLength(200);

            builder.Property(c => c.ProfileImagePath)
                .IsRequired(false)
                .HasMaxLength(200);

            builder.HasOne(c => c.WasalyIdentityUser)
                .WithOne()
                .HasForeignKey<Courier>(c => c.WasalyIdentityUserId);
        }
    }
    
    
}
