using EventManagementSystem.DAL.Entities;

namespace EventManagementSystem.DAL.Repositories.Interfaces
{
    public interface IEventTypeRepository : IGenericRepository<EventType>
    {
        Task<IEnumerable<EventType>> SearchEventTypeAsync(string searchTerm);

    }
}
