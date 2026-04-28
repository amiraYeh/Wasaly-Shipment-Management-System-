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
    public class LocationConfig: IEntityTypeConfiguration<Location>
    {
        public void Configure(EntityTypeBuilder<Location> builder)
        {
            builder.Property(x => x.Address)
                   .IsRequired();

            builder.Property(x => x.Latitude)
                   .IsRequired();

            builder.Property(x => x.Longitude)
                   .IsRequired();
        }
    }
}
