using EventManagementSystem.DAL.Data;
using EventManagementSystem.DAL.Entities;
using EventManagementSystem.DAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace EventManagementSystem.DAL.Repositories.Implementations
{
    public class LocationRepository : GenericRepository<Location>, ILocationRepository
    {
        public LocationRepository(EventManagementSystemDbContext context) : base(context)
        {
        }
        public async Task<Location?> GetByIdWithPhotosAndEventsAsync(int id)
        {
            return await _context.Locations
                                 .Include(l => l.locationPhotos)
                                 .Include(l => l.Events)
                                 .FirstOrDefaultAsync(l => l.Id == id);
        }

        public async Task<IEnumerable<Location>> SearchLocationsAsync(string searchTerm)
        {
            return await _context.Locations
                .Where(l => l.Name.Contains(searchTerm, StringComparison.OrdinalIgnoreCase) ||
                            l.Description.Contains(searchTerm, StringComparison.OrdinalIgnoreCase))
                .ToListAsync();
        }
        public async Task<IEnumerable<Location>> GetAllLocationsWithPhotosAsync()
        {
            return await _context.Locations
                .Include(l => l.locationPhotos)
                .ToListAsync();
        }

        public async Task<Location?> GetByIdWithPhotosAsync(int id)
        {
            return await _context.Locations
                .Include(l => l.locationPhotos)
                .FirstOrDefaultAsync(l => l.Id == id);
        }
    }
}