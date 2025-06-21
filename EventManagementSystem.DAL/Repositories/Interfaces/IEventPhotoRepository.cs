using EventManagementSystem.DAL.Entities;

namespace EventManagementSystem.DAL.Repositories.Interfaces
{
    public interface IEventPhotoRepository : IGenericRepository<EventPhoto>
    {
        Task<IEnumerable<EventPhoto>> GetPhotosByEventIdAsync(int eventId);
        Task<bool> DeletePhotosByEventIdAsync(int eventId);
    }
}
