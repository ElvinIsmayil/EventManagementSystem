using EventManagementSystem.DAL.Entities;

namespace EventManagementSystem.DAL.Repositories.Interfaces
{
    public interface ILocationRepository : IGenericRepository<Location>
    {
        Task<Location?> GetByIdWithPhotosAndEventsAsync(int id);
        Task<IEnumerable<Location>> GetAllLocationsWithPhotosAsync();
        Task<IEnumerable<Location>> SearchLocationsAsync(string searchTerm);

    }
}
