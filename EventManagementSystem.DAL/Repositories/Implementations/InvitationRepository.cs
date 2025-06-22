using EventManagementSystem.DAL.Data;
using EventManagementSystem.DAL.Entities;
using EventManagementSystem.DAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace EventManagementSystem.DAL.Repositories.Implementations
{
    public class InvitationRepository : GenericRepository<Invitation>, IInvitationRepository
    {
        public InvitationRepository(EventManagementSystemDbContext context) : base(context)
        {
        }
        public async Task<IEnumerable<Invitation>> GetByEventIdAsync(int eventId) // Renamed for clarity
        {
            return await _context.Invitations
                                 .Include(i => i.Person)
                                    .ThenInclude(p => p.AppUser) // Important for invitee details
                                                                 // .Include(i => i.Event) // Event is already part of the current context if filtering by EventId, but safe to include explicitly if you need its details for mapping
                                 .Where(i => i.EventId == eventId)
                                 .ToListAsync();
        }
        public async Task<IEnumerable<Invitation>> GetByPersonIdAsync(int personId)
        {
            // Crucial: Use .Include() to load navigation properties required by your InvitationDetailsVM
            // This prevents null reference exceptions during AutoMapper mapping.
            return await _context.Invitations
                                 .Include(i => i.Event)           // Include the associated Event
                                 .Include(i => i.Person)          // Include the associated Person
                                     .ThenInclude(p => p.AppUser) // And then include the AppUser of that Person
                                 .Where(i => i.PersonId == personId)
                                 .ToListAsync();
        }
        // Inside InvitationRepository
        public async Task<Invitation?> GetByCodeAsync(string invitationCode) // Make return type nullable
        {
            return await _context.Invitations
                                 .Include(i => i.Event)
                                 .Include(i => i.Person)
                                    .ThenInclude(p => p.AppUser)
                                 .FirstOrDefaultAsync(i => i.InvitationCode == invitationCode);
        }
    }
}
