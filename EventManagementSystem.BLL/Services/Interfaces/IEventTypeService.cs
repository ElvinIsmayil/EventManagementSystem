using EventManagementSystem.BLL.ViewModels.EventType;

namespace EventManagementSystem.BLL.Services.Interfaces
{
    public interface IEventTypeService
    {
        Task<EventTypeDetailVM> AddAsync(EventTypeCreateVM viewModel);
        Task<EventTypeDetailVM> UpdateAsync(EventTypeUpdateVM viewModel);
        Task<bool> DeleteAsync(int id);
        Task<EventTypeDetailVM?> GetByIdAsync(int id);
        Task<IEnumerable<EventTypeListVM>> GetAllAsync();
        Task<EventTypeUpdateVM?> GetUpdateByIdAsync(int id);
        Task<IEnumerable<EventTypeListVM>> SearchEventTypeAsync(string searchTerm);


    }
}
