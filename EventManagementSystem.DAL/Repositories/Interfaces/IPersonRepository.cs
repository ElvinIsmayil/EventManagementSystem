using EventManagementSystem.DAL.Entities;

namespace EventManagementSystem.DAL.Repositories.Interfaces
{
    public interface IPersonRepository : IGenericRepository<Person>
    {
        Task<int?> GetPersonIdByAppUserIdAsync(string appUserId);
        Task<Person?> GetPersonByAppUserIdAsync(string appUserId);
        Task<string?> GetPersonFullNameAsync(int personId);
        Task<string?> GetPersonEmailAsync(int personId);
        Task<IEnumerable<Person>> GetWithInvitationsAndNotificationsAsync(IEnumerable<string> userIds);
        Task<Person?> GetByAppUserIdWithInvitationsAndNotificationsAsync(string appUserId);
    }
}
