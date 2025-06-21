using EventManagementSystem.BLL.ViewModels.Organizer;

namespace EventManagementSystem.BLL.Services.Interfaces
{
    public interface IOrganizerService
    {

        Task<OrganizerDetailsVM?> GetByIdAsync(int id);
        Task<OrganizerDetailsVM?> GetOrganizerByAppUserIdAsync(string appUserId);
        Task<IEnumerable<OrganizerListVM>> GetAllAsync();
        Task<OrganizerUpdateVM?> GetUpdateByIdAsync(int id);

        Task<bool> AddAsync(OrganizerCreateVM model);
        Task<bool> UpdateAsync(OrganizerUpdateVM model);
        Task<bool> DeleteAsync(int id);
        Task<bool> ApproveOrganizerAsync(int id);
        Task<IEnumerable<OrganizerListVM>> SearchAsync(string searchTerm);
    }
}
