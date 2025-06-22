using EventManagementSystem.DAL.Data;
using EventManagementSystem.DAL.Entities;
using EventManagementSystem.DAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventManagementSystem.DAL.Repositories.Implementations
{
    public class ParticipationRepository : GenericRepository<Participation>, IParticipationRepository
    {
        public ParticipationRepository(EventManagementSystemDbContext context) : base(context)
        {
        }
        public async Task<Participation?> GetByInvitationIdAsync(int invitationId)
        {
            return await _context.Participations
                .FirstOrDefaultAsync(p => p.InvitationId == invitationId);
        }
        public async Task<IEnumerable<Participation>> GetByEventIdAsync(int eventId)
        {
            return await _context.Participations
                .Where(p => p.Invitation.EventId == eventId)
                .ToListAsync();
        }
        public async Task<IEnumerable<Participation>> GetByPersonIdAsync(int personId)
        {
            return await _context.Participations
                .Where(p => p.Invitation.PersonId == personId)
                .ToListAsync();
        }

        public async Task<Participation?> GetByConfirmationCodeAsync(string confirmationCode)
        {
            return await _context.Participations
                .FirstOrDefaultAsync(p => p.ConfirmationCode == confirmationCode);
        }

        public async Task<bool> IsSeatAvailableAsync(int eventId, int seatNumber)
        {
            return !await _context.Participations
                .AnyAsync(p => p.Invitation.EventId == eventId && p.SeatNumber == seatNumber);

        }

       


    }

}
