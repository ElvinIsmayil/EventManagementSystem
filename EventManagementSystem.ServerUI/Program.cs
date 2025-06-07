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

            // Add services to the container.
            builder.Services.AddControllersWithViews();
            builder.Services.RegisterDataAccessServices(config);


            var app = builder.Build();

            using var scope = app.Services.CreateScope();
            var services = scope.ServiceProvider;

            try
            {
                // Resolve the ApplicationDbSeeder from the service provider.
                // Its dependencies (DbContext, UserManager, RoleManager) will be automatically injected.
                var dbSeeder = services.GetRequiredService<ApplicationDbSeeder>();
                await dbSeeder.SeedAsync(); // <--- Call the seed operation
            }
            catch (Exception ex)
            {
                var logger = services.GetRequiredService<ILogger<Program>>();
                logger.LogError(ex, "An error occurred during database migration and/or seeding.");
                // Depending on severity, you might want to re-throw: throw;
            }


            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseRouting();

            app.UseAuthorization();

            app.MapStaticAssets();
            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}")
                .WithStaticAssets();

            app.Run();
        }
    }
}
