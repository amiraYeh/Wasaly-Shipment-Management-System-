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
    internal class MerchantConfig:IEntityTypeConfiguration<Merchant>
    {
        public void Configure(EntityTypeBuilder<Merchant> builder)
        {

            builder.Property(c => c.StoreName)
           .IsRequired()
           .HasMaxLength(100);

            builder.Property(c => c.StoreAddress)
                    .IsRequired()
                    .HasMaxLength(200);

            builder.Property(c => c.BusinessType)
                .IsRequired()
                .HasMaxLength(50);

            builder.HasOne(c => c.WasalyIdentityUser)
                .WithOne()
                .HasForeignKey<Merchant>(c => c.WasalyIdentityUserId);


        }
    }

}
