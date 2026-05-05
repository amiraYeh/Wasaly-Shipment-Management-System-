using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Wasaly.BLL.Services;
using Wasaly.DAL.Data.Context;
using Wasaly.DAL.Models;
using Wasaly.DAL.Repositories;
using Wasaly.DAL.Repositories.IRepositories;
using Wasaly.PL.Extensions;

//using Wasaly.BLL.Services;
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


            builder.Services.AddRazorPages();
            builder.Services.ConfigureApplicationCookie(options =>
            {
                options.LoginPath = "/Identity/Account/Login";
                options.AccessDeniedPath = "/Identity/Account/AccessDenied";
            });
            builder.Services.AddScoped<IUserRepository, UserRepository>();
           

            var app = builder.Build();

            using (var scope = app.Services.CreateScope())
            {
                var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
                var roles = new[] { "Admin", "Merchant", "Courier" };

                foreach (var role in roles)
                {
                    if (!await roleManager.RoleExistsAsync(role))
                    {
                        await roleManager.CreateAsync(new IdentityRole(role));
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
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
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
