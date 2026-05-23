using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Wasaly.DAL.Data.Context;
using Wasaly.DAL.Models;

namespace Wasaly.PL.Data
{
    public static class ApplicationDbSeeder
    {
        public static async Task SeedAsync(IServiceProvider services, IConfiguration configuration)
        {
            var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
            var userManager = services.GetRequiredService<UserManager<WasalyIdentityUser>>();
            var db = services.GetRequiredService<ApplicationDbContext>();

            var roles = new[] { "Admin", "Merchant", "Courier" };

            foreach (var role in roles)
            {
                if (!await roleManager.RoleExistsAsync(role))
                {
                    await roleManager.CreateAsync(new IdentityRole(role));
                }
            }

            var adminEmail = configuration["Seed:AdminEmail"] ?? "admin@wasaly.local";
            var adminPassword = configuration["Seed:AdminPassword"] ?? "Admin@12345";

            var existing = await userManager.FindByEmailAsync(adminEmail);

            if (existing == null)
            {
                var adminLocation = new Location
                {
                    Address = "Admin HQ",
                    Latitude = 0,
                    Longitude = 0
                };

                db.Locations.Add(adminLocation);
                await db.SaveChangesAsync();

                var adminUser = new WasalyIdentityUser
                {
                    UserName = adminEmail,
                    Email = adminEmail,
                    EmailConfirmed = true,
                    FullName = "Administrator",
                    PhoneNumber = "0000000000",
                    LocationId = adminLocation.Id
                };

                var result = await userManager.CreateAsync(adminUser, adminPassword);

                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(adminUser, "Admin");
                }
            }
        }
    }
}
