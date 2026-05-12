using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Wasaly.BLL.@interface;
using Wasaly.BLL.Services;
using Wasaly.BLL.Services.Interfaces;
using Wasaly.BLL.Settings;
using Wasaly.DAL.Data.Context;
using Wasaly.DAL.Enums;
using Wasaly.DAL.Models;
using Wasaly.DAL.Repositories;
using Wasaly.DAL.Repositories.IRepositories;
using Wasaly.PL.Extensions;

namespace Wasaly.PL
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
            builder.Services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(connectionString));
            //builder.Services.AddDatabaseDeveloperPageExceptionFilter();

            builder.Services.AddScoped<IShipmentService, ShipmentService>();
            builder.Services.AddScoped<IGoogleMapService,GoogleMapService>();
            builder.Services.AddScoped<IShipmentRepository, ShipmentRepository>();

            builder.Services.AddIdentity<WasalyIdentityUser, IdentityRole>(options => options.SignIn.RequireConfirmedAccount = false)
                .AddEntityFrameworkStores<ApplicationDbContext>();
            builder.Services.AddControllersWithViews();
            builder.Services.AddCourierServices(builder.Configuration);
            builder.Services.Configure<EmailSettings>(builder.Configuration.GetSection("EmailSettings"));
            builder.Services.AddScoped<IEmailService, EmailService>();

            builder.Services.AddRazorPages();
            builder.Services.ConfigureApplicationCookie(options =>
            {
                options.LoginPath = "/Identity/Account/Login";
                options.AccessDeniedPath = "/Identity/Account/AccessDenied";
            });
            builder.Services.AddScoped<IUserRepository, UserRepository>();
            builder.Services.AddScoped<IAdminService, AdminService>();
            var app = builder.Build();

            using (var scope = app.Services.CreateScope())
            {
                var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
                var userManager = scope.ServiceProvider.GetRequiredService<UserManager<WasalyIdentityUser>>();
                var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                var roles = new[] { "Admin", "Merchant", "Courier" };

                foreach (var role in roles)
                {
                    if (!await roleManager.RoleExistsAsync(role))
                    {
                        await roleManager.CreateAsync(new IdentityRole(role));
                    }
                }

                // Seed default admin user (only if not exists)
                var adminEmail = builder.Configuration["Seed:AdminEmail"] ?? "admin@wasaly.local";
                var adminPassword = builder.Configuration["Seed:AdminPassword"] ?? "Admin@12345";
                var existing = await userManager.FindByEmailAsync(adminEmail);
                if (existing == null)
                {
                    // Ensure there's a Location for required FK (adjust address/coords as needed)
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

                    var createResult = await userManager.CreateAsync(adminUser, adminPassword);
                    if (createResult.Succeeded)
                    {
                        await userManager.AddToRoleAsync(adminUser, "Admin");
                    }
                    else
                    {
                        // optional: log/create diagnostics here
                    }
                }
            }

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseMigrationsEndPoint();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();
            app.MapRazorPages();
            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}
