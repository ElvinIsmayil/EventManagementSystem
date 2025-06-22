using EventManagementSystem.DAL.Entities;

namespace EventManagementSystem.DAL.Repositories.Interfaces
{
    public interface IEventRepository : IGenericRepository<Event>
    {
        Task<Event?> GetByIdWithPhotosAndLocationAsync(int id);
        Task<IEnumerable<Event>> GetAllEventsWithPhotosAsync();
        Task<IEnumerable<Event>> SearchEventsAsync(string searchTerm);
        Task<IEnumerable<Event>> GetUpcomingEventsAsync(DateTime startDate, DateTime endDate);
        Task<IEnumerable<Event>> GetPastEventsAsync(DateTime startDate, DateTime endDate);
        Task<Event> GetWithLocationAsync(int id);
        Task<IEnumerable<Event>> GetEventsByOrganizerIdAsync(int organizerId);
    }
}
