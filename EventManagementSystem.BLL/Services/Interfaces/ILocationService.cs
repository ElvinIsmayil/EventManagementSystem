using EventManagementSystem.BLL.ViewModels.Location;

namespace EventManagementSystem.BLL.Services.Interfaces
{
    public interface ILocationService
    {
        Task<LocationDetailsVM?> AddAsync(LocationCreateVM viewModel);
        Task<LocationDetailsVM?> UpdateAsync(LocationUpdateVM viewModel);
        Task<bool> DeleteAsync(int id);
        Task<LocationDetailsVM?> GetByIdAsync(int id);
        Task<LocationUpdateVM?> GetUpdateByIdAsync(int id);
        Task<IEnumerable<LocationListVM>> GetAllAsync();
        Task<IEnumerable<LocationListVM>> GetAllLocationsWithPhotosAsync();
        Task<IEnumerable<LocationListVM>> SearchLocationsAsync(string searchTerm);
    }
}
