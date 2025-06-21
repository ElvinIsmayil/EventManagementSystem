using EventManagementSystem.DAL.Data;
using EventManagementSystem.DAL.DataSeeding;
using EventManagementSystem.DAL.Entities;
using EventManagementSystem.DAL.Repositories.Implementations;
using EventManagementSystem.DAL.Repositories.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace EventManagementSystem.DAL
{
    public static class DataAccessModule
    {
        public static void RegisterDataAccessServices(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            services.AddDbContext<EventManagementSystemDbContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

            services.AddIdentity<AppUser, IdentityRole>(options =>
            {
                options.Password.RequireDigit = false;
                options.Password.RequiredLength = 6;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireLowercase = false;
                options.SignIn.RequireConfirmedEmail = false;
            })
            .AddEntityFrameworkStores<EventManagementSystemDbContext>()
            .AddDefaultTokenProviders();

            services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            services.AddScoped<IEventTypeRepository, EventTypeRepository>();
            services.AddScoped<ILocationRepository, LocationRepository>();
            services.AddScoped<ILocationPhotoRepository, LocationPhotoRepository>();
            services.AddScoped<IEventPhotoRepository, EventPhotoRepository>();
            services.AddScoped<IEventRepository, EventRepository>();
            services.AddScoped<IOrganizerRepository, OrganizerRepository>();
            services.AddScoped<IPersonRepository, PersonRepository>();  


            services.AddScoped<ApplicationDbSeeder>();
        }
    }
}