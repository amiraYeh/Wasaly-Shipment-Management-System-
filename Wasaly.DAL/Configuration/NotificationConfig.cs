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
    public class NotificationConfig:IEntityTypeConfiguration<Notification>
    {
        public void Configure(EntityTypeBuilder<Notification> builder)
        {
            builder.Property(x => x.Title)
                   .IsRequired()
                   .HasMaxLength(50);

            builder.Property(x => x.Message)
                   .IsRequired()
                     .HasMaxLength(500);

            builder.Property(x => x.IsRead)
                   .HasDefaultValue(false);

            builder.HasOne(n => n.WasalyIdentityUser)
            .WithMany(u => u.notifications)
            .HasForeignKey(n => n.WasalyIdentityUserId);
        }
    }
}
