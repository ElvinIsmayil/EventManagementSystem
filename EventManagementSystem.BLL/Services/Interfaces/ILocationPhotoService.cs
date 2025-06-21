using EventManagementSystem.BLL.ViewModels.LocationPhoto;

namespace EventManagementSystem.BLL.Services.Interfaces
{
    public interface ILocationPhotoService
    {
        Task<List<LocationPhotoDetailsVM>> GetPhotosByLocationIdAsync(int locationId);

        Task<LocationPhotoDetailsVM?> AddPhotoToLocationAsync(int locationId, LocationPhotoCreateVM viewModel);

        Task<bool> DeletePhotoFromLocationAsync(int photoId, int locationId);

        Task<LocationPhotoDetailsVM?> UpdatePhotoAsync(LocationPhotoUpdateVM viewModel);

        Task<List<LocationPhotoDetailsVM>> GetAllPhotosAsync();
    }
}