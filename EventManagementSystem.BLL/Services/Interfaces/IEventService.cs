using EventManagementSystem.BLL.ViewModels.Event;

namespace EventManagementSystem.BLL.Services.Interfaces
{
    public interface IEventService
    {
        Task<IEnumerable<EventListVM>> GetAllAsync();
        Task<EventDetailsVM?> GetByIdAsync(int id);
        Task<EventUpdateVM> GetUpdateByIdAsync(int id);

        Task<EventDetailsVM> AddAsync(EventCreateVM model);
        Task<EventDetailsVM> UpdateAsync(EventUpdateVM model);
        Task<bool> DeleteAsync(int id);
        Task<IEnumerable<EventListVM>> SearchEventsAsync(string searchTerm);
        Task<IEnumerable<EventListVM>> GetUpcomingEventsAsync(DateTime startDate, DateTime endDate);
        Task<IEnumerable<EventListVM>> GetPastEventsAsync(DateTime startDate, DateTime endDate);
    }
}
