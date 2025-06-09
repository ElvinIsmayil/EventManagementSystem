using EventManagementSystem.DAL.Data;
using EventManagementSystem.DAL.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace EventManagementSystem.DAL.DataSeeding
{
    public class ApplicationDbSeeder
    {
        private readonly EventManagementSystemDbContext _context;
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public ApplicationDbSeeder(
            EventManagementSystemDbContext context,
            UserManager<AppUser> userManager,
            RoleManager<IdentityRole> roleManager)
        {
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task SeedAsync()
        {
            await _context.Database.MigrateAsync();

            await SeedRolesAsync();

            await SeedAdminUserAsync();

            await SeedEventTypesAsync();

            await SeedLocationsAsync();
        }

        private async Task SeedRolesAsync()
        {
            if (!await _roleManager.RoleExistsAsync("SuperAdmin"))
            {
                await _roleManager.CreateAsync(new IdentityRole("SuperAdmin"));
            }
            if (!await _roleManager.RoleExistsAsync("Admin"))
            {
                await _roleManager.CreateAsync(new IdentityRole("Admin"));
            }
            if (!await _roleManager.RoleExistsAsync("Organizer"))
            {
                await _roleManager.CreateAsync(new IdentityRole("Organizer"));
            }
            if (!await _roleManager.RoleExistsAsync("Teacher"))
            {
                await _roleManager.CreateAsync(new IdentityRole("Teacher"));
            }
            if (!await _roleManager.RoleExistsAsync("Student"))
            {
                await _roleManager.CreateAsync(new IdentityRole("Student"));
            }
            if (!await _roleManager.RoleExistsAsync("Guest"))
            {
                await _roleManager.CreateAsync(new IdentityRole("Guest"));
            }
        }

        private async Task SeedAdminUserAsync()
        {
            var adminUser = await _userManager.FindByEmailAsync("admin@gmail.com");
            if (adminUser == null)
            {
                adminUser = new AppUser
                {
                    UserName = "admin@gmail.com",
                    Email = "admin@gmail.com",
                    EmailConfirmed = true,
                    IsActive = true,
                    CreatedAt = DateTime.UtcNow,
                    Name = "Elvin",
                    Surname = "Ismayil",
                    PhoneNumber = "0502140236",
                    DateOfBirth = new DateTime(2007, 12, 22),
                    ImageUrl = "https://www.pngmart.com/files/21/Admin-Profile-Vector-PNG-Image.png"
                };

                var result = await _userManager.CreateAsync(adminUser, "Admin123!");
                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(adminUser, "SuperAdmin");
                }
            }
        }

        private async Task SeedEventTypesAsync()
        {
            if (!await _context.EventTypes.AnyAsync())
            {
                _context.EventTypes.AddRange(
                    new EventType { Name = "Conference", Description = "Large-scale academic or professional gathering." },
                    new EventType { Name = "Workshop", Description = "Interactive session for practical skills." },
                    new EventType { Name = "Seminar", Description = "Educational presentation by an expert." },
                    new EventType { Name = "Webinar", Description = "Online seminar." },
                    new EventType { Name = "Concert", Description = "Live musical performance." }
                );
                await _context.SaveChangesAsync();
            }
        }

        private async Task SeedLocationsAsync()
        {
            if (!await _context.Locations.AnyAsync())
            {
                _context.Locations.AddRange(
                    new Location { Name = "Grand Convention Hall", Description = "Large hall for conferences", Address = "123 Convention St, City", Capacity = 5000 },
                    new Location { Name = "Tech Hub Auditorium", Description = "Modern auditorium for tech talks", Address = "456 Innovation Ave, City", Capacity = 500 },
                    new Location { Name = "Community Center Hall", Description = "Versatile space for local events", Address = "789 Main Rd, Town", Capacity = 150 }
                );
                await _context.SaveChangesAsync();
            }
        }
    }
}