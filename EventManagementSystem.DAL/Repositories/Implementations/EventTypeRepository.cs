using EventManagementSystem.DAL.Data;
using EventManagementSystem.DAL.Entities;
using EventManagementSystem.DAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace EventManagementSystem.DAL.Repositories.Implementations
{
    public class EventTypeRepository : GenericRepository<EventType>, IEventTypeRepository
    {
        public EventTypeRepository(EventManagementSystemDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<EventType>> SearchEventTypeAsync(string searchTerm)
        {
            var eventTypesQuery = _context.EventTypes.AsQueryable();

            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                var lowerSearch = searchTerm.Trim().ToLowerInvariant();
                eventTypesQuery = eventTypesQuery.Where(m =>
                m.Name.ToLower().Contains(lowerSearch) ||
                m.Description.ToLower().Contains(lowerSearch));

            }
            var eventTypesResult = await eventTypesQuery.ToListAsync();
            return eventTypesResult;

        }
    }


}
