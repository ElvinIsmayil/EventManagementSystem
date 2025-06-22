using EventManagementSystem.DAL.Entities;

namespace EventManagementSystem.DAL.Repositories.Interfaces
{
    public interface IInvitationRepository : IGenericRepository<Invitation>
    {
        Task<IEnumerable<Invitation>> GetByEventIdAsync(int eventId);
        Task<IEnumerable<Invitation>> GetByPersonIdAsync(int personId);
        Task<Invitation> GetByCodeAsync(string invitationCode);

    }
}
