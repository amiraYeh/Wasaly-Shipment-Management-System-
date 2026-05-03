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
    internal class WasalyIdentityUserConfig : IEntityTypeConfiguration<WasalyIdentityUser>
    {
  
        public void Configure(EntityTypeBuilder<WasalyIdentityUser> builder)
        {
           

            builder.Property(u => u.FullName)
                .IsRequired()
                .HasMaxLength(30);

           

            builder.Property(u => u.PhoneNumber)
                .IsRequired();

            builder.HasIndex(u => u.PhoneNumber)
                .IsUnique();

            builder.HasOne(u => u.Location)
             .WithMany(l => l.Users)
             .HasForeignKey(u => u.LocationId)
             .OnDelete(DeleteBehavior.Restrict);


                //// Location is a navigation (Location). Do not treat it as a scalar property.
                //// Configure Region (string) instead if you intended a max length constraint.
                //builder.Property(u => u.Region)
                //    .IsRequired()
                //    .HasMaxLength(100);

               

                    
           


    }
    }
}
