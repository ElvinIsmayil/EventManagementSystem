using EventManagementSystem.DAL.Entities;

namespace EventManagementSystem.BLL.Services.Interfaces
{
    public interface IEventService
    {
        //Task<IEnumerable<EventListVM>> GetAllEventsAsync();
        Task<Event?> GetEventByIdAsync(int id);
        Task<Event?> CreateEventAsync(Event newEvent);
        Task<Event?> UpdateEventAsync(Event updatedEvent);
        Task<bool> DeleteEventAsync(int id);
        Task<IEnumerable<Event>> SearchEventsAsync(string searchTerm);
        Task<IEnumerable<Event>> GetUpcomingEventsAsync(DateTime startDate, DateTime endDate);
        Task<IEnumerable<Event>> GetPastEventsAsync(DateTime startDate, DateTime endDate);
    }
}
