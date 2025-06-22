using EventManagementSystem.DAL.Entities;
using EventManagementSystem.DAL.Repositories.Implementations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventManagementSystem.DAL.Repositories.Interfaces
{
    public interface IParticipationRepository : IGenericRepository<Participation>
    {
        Task<Participation> GetByInvitationIdAsync(int invitationId);
        Task<Participation> GetByConfirmationCodeAsync(string confirmationCode);
        Task<IEnumerable<Participation>> GetByEventIdAsync(int eventId);
        Task<IEnumerable<Participation>> GetByPersonIdAsync(int personId);
        Task<bool> IsSeatAvailableAsync(int eventId, int seatNumber);
       // Task<Participation?> GetByEventAndPersonAsync(int eventId, int personId);

    }
}
