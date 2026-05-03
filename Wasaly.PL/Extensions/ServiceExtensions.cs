using System.Configuration;
using Wasaly.BLL.Services;
using Wasaly.BLL.Services.Interfaces;
using Wasaly.BLL.Settings;
using Wasaly.DAL.Repositories;
using Wasaly.DAL.Repositories.IRepositories;

namespace Wasaly.PL.Extensions
{
    public static class ServiceExtensions
    {
        public static IServiceCollection AddCourierServices(
     this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<ICourierRepository, CourierRepository>();
            services.AddScoped<ICourierService, CourierService>();

            // Email
            services.Configure<EmailSettings>(
                configuration.GetSection("EmailSettings"));
            services.AddScoped<IEmailService, EmailService>();

            return services;
        }
    }
}
