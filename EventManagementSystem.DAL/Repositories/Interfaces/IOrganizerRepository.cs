using EventManagementSystem.DAL.Entities;

namespace EventManagementSystem.DAL.Repositories.Interfaces
{
    public interface IOrganizerRepository : IGenericRepository<Organizer>
    {
        Task<Organizer?> GetOrganizerByIdWithAppUserAsync(int id);
        Task<IEnumerable<Organizer>> GetAllOrganizersWithAppUserAsync();
        Task<Organizer?> GetOrganizerByAppUserIdAsync(string appUserId);
        Task<IEnumerable<Organizer>> SearchAsync(string searchTerm);

    }
}
