using EventManagementSystem.DAL.Data;
using EventManagementSystem.DAL.Entities;
using EventManagementSystem.DAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace EventManagementSystem.DAL.Repositories.Implementations
{
    public class EventPhotoRepository : GenericRepository<EventPhoto>, IEventPhotoRepository
    {
        public EventPhotoRepository(EventManagementSystemDbContext context) : base(context)
        {
        }
        public async Task<IEnumerable<EventPhoto>> GetPhotosByEventIdAsync(int eventId)
        {
            return await _context.EventPhotos
                .Where(ep => ep.EventId == eventId)
                .ToListAsync();
        }

        public async Task<bool> DeletePhotosByEventIdAsync(int id)
        {
            var photo = await _context.EventPhotos.FindAsync(id);
            if (photo == null)
            {
                return false;
            }
            _context.EventPhotos.Remove(photo);
            return await _context.SaveChangesAsync() > 0;
        }


    }
}
