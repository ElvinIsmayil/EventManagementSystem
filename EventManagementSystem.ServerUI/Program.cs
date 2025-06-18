using EventManagementSystem.BLL;
using EventManagementSystem.DAL;
using EventManagementSystem.DAL.DataSeeding;

namespace EventManagementSystem.ServerUI
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            var config = builder.Configuration;

            builder.Services.AddControllersWithViews().AddRazorRuntimeCompilation();
            builder.Services.RegisterDataAccessServices(config);
            builder.Services.RegisterServices(config);

            var app = builder.Build();

            bool seedDatabase = config.GetValue<bool>("SeedDatabase");

            if (seedDatabase)
            {
                using var scope = app.Services.CreateScope();
                var services = scope.ServiceProvider;

                try
                {
                    var dbSeeder = services.GetRequiredService<ApplicationDbSeeder>();
                    await dbSeeder.SeedAsync();
                }
                catch (Exception ex)
                {
                    var logger = services.GetRequiredService<ILogger<Program>>();
                    logger.LogError(ex, "An error occurred during database migration and/or seeding.");
                }

            }

            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapStaticAssets();
            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Auth}/{action=SignIn}/{id?}")
                .WithStaticAssets();

            app.Run();
        }
    }
}
