using EventManagementSystem.DAL.Entities;

namespace EventManagementSystem.DAL.Repositories.Interfaces
{
    public interface ILocationPhotoRepository : IGenericRepository<LocationPhoto>
    {
        Task<List<LocationPhoto>> GetPhotosByLocationIdAsync(int locationId);

    }
}
