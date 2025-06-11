using EventManagementSystem.BLL.ViewModels.LocationPhoto;

namespace EventManagementSystem.BLL.Services.Interfaces
{
    public interface ILocationPhotoService
    {
        Task<List<LocationPhotoListVM>> GetPhotosByLocationIdAsync(int locationId);

        Task<LocationPhotoListVM?> AddPhotoToLocationAsync(int locationId, LocationPhotoCreateVM viewModel);

        Task<bool> DeletePhotoFromLocationAsync(int photoId, int locationId);

        Task<LocationPhotoListVM?> UpdatePhotoAsync(LocationPhotoUpdateVM viewModel);

        Task<List<LocationPhotoListVM>> GetAllPhotosAsync();
    }
}