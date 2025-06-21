using EventManagementSystem.BLL.ViewModels.EventPhoto;

namespace EventManagementSystem.BLL.Services.Interfaces
{
    public interface IEventPhotoService
    {
        Task<EventPhotoDetailsVM> AddAsync(int eventId, EventPhotoCreateVM model);
        Task<EventPhotoDetailsVM?> GetByIdAsync(int photoId);
        Task<IEnumerable<EventPhotoDetailsVM>> GetPhotosByEventIdAsync(int eventId);
        Task<EventPhotoDetailsVM?> UpdateAsync(EventPhotoUpdateVM model);
        Task<bool> DeleteAsync(int photoId);

        // Optional: If you need to manage the 'Order' of photos explicitly, you might add:
        // Task<bool> ReorderPhotosAsync(int eventId, List<PhotoOrderViewModel> photoOrders);
        // (where PhotoOrderViewModel would be { int PhotoId, int NewOrder })
    }
}
