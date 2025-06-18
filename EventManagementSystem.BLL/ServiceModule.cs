using EventManagementSystem.BLL.Configurations;
using EventManagementSystem.BLL.Infrastructure.Implementations;
using EventManagementSystem.BLL.Infrastructure.Interfaces;
using EventManagementSystem.BLL.Profiles;
using EventManagementSystem.BLL.Services.Implementations;
using EventManagementSystem.BLL.Services.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace EventManagementSystem.BLL
{
    public static class ServiceModule
    {
        public static void RegisterServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddAutoMapper(typeof(CustomProfile).Assembly);
            services.Configure<SmtpSettings>(configuration.GetSection("SmtpSettings"));
            services.AddSingleton<IEmailService, EmailService>();
            services.AddScoped<IImageService, ImageService>();

            services.AddScoped<IAuthenticationService, AuthenticationService>();
            services.AddScoped<IEventTypeService, EventTypeService>();
            services.AddScoped<ILocationService, LocationService>();
            services.AddScoped<ILocationPhotoService, LocationPhotoService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IProfileService, ProfileService>();

        }
    }
}
