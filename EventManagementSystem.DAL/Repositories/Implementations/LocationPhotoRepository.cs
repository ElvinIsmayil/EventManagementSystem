using EventManagementSystem.DAL.Data;
using EventManagementSystem.DAL.Entities;
using EventManagementSystem.DAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace EventManagementSystem.DAL.Repositories.Implementations
{
    public class LocationPhotoRepository : GenericRepository<LocationPhoto>, ILocationPhotoRepository
    {
        public LocationPhotoRepository(EventManagementSystemDbContext context) : base(context)
        {
        }
        public async Task<List<LocationPhoto>> GetPhotosByLocationIdAsync(int locationId)
        {
            return await _context.LocationPhotos
                                .Where(p => p.LocationId == locationId)
                                .ToListAsync();
        }
    }
}
