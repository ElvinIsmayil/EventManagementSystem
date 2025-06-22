using EventManagementSystem.DAL.Data;
using EventManagementSystem.DAL.Entities;
using EventManagementSystem.DAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace EventManagementSystem.DAL.Repositories.Implementations
{
    public class EventRepository : GenericRepository<Event>, IEventRepository
    {
        public EventRepository(EventManagementSystemDbContext context) : base(context)
        {
        }
        public async Task<IEnumerable<Event>> SearchEventsAsync(string searchTerm)
        {
            return await _context.Events
                .Where(e => e.Title.Contains(searchTerm, StringComparison.OrdinalIgnoreCase) ||
                            e.Description.Contains(searchTerm, StringComparison.OrdinalIgnoreCase))
                .ToListAsync();
        }
        public async Task<Event?> GetByIdWithPhotosAndLocationAsync(int id)
        {
            return await _context.Events
                .Include(e => e.EventPhotos)
                .Include(e => e.Location)
                .FirstOrDefaultAsync(e => e.Id == id);
        }

        public async Task<IEnumerable<Event>> GetAllEventsWithPhotosAsync()
        {
            return await _context.Events
                .Include(e => e.EventPhotos)
                .ToListAsync();
        }

        public async Task<IEnumerable<Event>> GetUpcomingEventsAsync(DateTime startDate, DateTime endDate)
        {
            return await _context.Events
                .Where(e => e.StartDate >= startDate && e.StartDate <= endDate)
                .ToListAsync();
        }

        public async Task<IEnumerable<Event>> GetPastEventsAsync(DateTime startDate, DateTime endDate)
        {
            return await _context.Events
                .Where(e => e.EndDate < startDate || e.EndDate > endDate)
                .ToListAsync();

        }
        public async Task<IEnumerable<Event>> GetAllEventsWithPhotosAndLocationAsync()
        {
            return await _context.Events
                .Include(e => e.EventPhotos)
                .Include(e => e.Location)
                .ToListAsync();
        }

        public async Task<Event> GetWithLocationAsync(int id)
        {
            return await _context.Events
                .Include(e => e.Location)
                .FirstOrDefaultAsync(e => e.Id == id);
        }

        public async Task<IEnumerable<Event>> GetEventsByOrganizerIdAsync(int organizerId)
        {
            return await _context.Events
                .Where(e => e.OrganizerId == organizerId)
                .ToListAsync();
        }


    }
}