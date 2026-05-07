using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection;
using Wasaly.DAL.Models;

namespace Wasaly.DAL.Data.Context
{

    public class ApplicationDbContext : IdentityDbContext<WasalyIdentityUser, IdentityRole, string>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
            base.OnModelCreating(builder);
            builder.Entity<Courier>()
          .Property(c => c.Balance)
          .HasColumnType("decimal(18,2)");

            builder.Entity<Shipment>()
                   .Property(s => s.Price)
                   .HasColumnType("decimal(18,2)");
        }

        public virtual DbSet<Courier> Couriers { get; set; }
        public virtual DbSet<Merchant> Merchants { get; set; }
        public virtual DbSet<Shipment> Shipments { get; set; }
        public virtual DbSet<ShipmentTracking> ShipmentTrackings { get; set; }
        public virtual DbSet<DeliveryOTP> DeliveryOTP { get; set; }
        public virtual DbSet<Location> Locations { get; set; }

        public virtual DbSet<CourierAssignment> CourierAssignments { get; set; }


    }
}
