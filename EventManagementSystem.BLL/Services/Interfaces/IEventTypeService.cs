using EventManagementSystem.BLL.ViewModels.EventType;

namespace EventManagementSystem.BLL.Services.Interfaces
{
    public interface IEventTypeService
    {
        Task<EventTypeDetailsVM> AddAsync(EventTypeCreateVM viewModel);
        Task<EventTypeDetailsVM> UpdateAsync(EventTypeUpdateVM viewModel);
        Task<bool> DeleteAsync(int id);
        Task<EventTypeDetailsVM?> GetByIdAsync(int id);
        Task<IEnumerable<EventTypeListVM>> GetAllAsync();
        Task<EventTypeUpdateVM?> GetUpdateByIdAsync(int id);
        Task<IEnumerable<EventTypeListVM>> SearchEventTypeAsync(string searchTerm);


    }
}
