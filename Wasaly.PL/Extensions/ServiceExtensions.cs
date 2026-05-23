using Wasaly.BLL.@interface;
using Wasaly.BLL.Services;
using Wasaly.BLL.Services.Interfaces;
using Wasaly.BLL.Settings;
using Wasaly.DAL.Repositories;
using Wasaly.DAL.Repositories.IRepositories;

namespace Wasaly.PL.Extensions
{
    public static class ServiceExtensions
    {
        public static IServiceCollection AddApplicationServices(
     this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<ICourierRepository, CourierRepository>();
            services.AddScoped<ICourierService, CourierService>();
            services.AddScoped<IShipmentService, ShipmentService>();
            services.AddScoped<IGoogleMapService, GoogleMapService>();
            services.AddScoped<IShipmentRepository, ShipmentRepository>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IAdminService, AdminService>();

            // Email
            services.Configure<EmailSettings>(
                configuration.GetSection("EmailSettings"));
            services.AddScoped<IEmailService, EmailService>();

            return services;
        }
    }
}
