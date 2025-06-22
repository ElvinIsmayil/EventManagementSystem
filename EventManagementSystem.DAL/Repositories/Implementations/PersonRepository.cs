using EventManagementSystem.DAL.Data;
using EventManagementSystem.DAL.Entities;
using EventManagementSystem.DAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace EventManagementSystem.DAL.Repositories.Implementations
{
    public class PersonRepository : GenericRepository<Person>, IPersonRepository
    {
        public PersonRepository(EventManagementSystemDbContext context) : base(context)
        {
        }

        public async Task<int?> GetPersonIdByAppUserIdAsync(string appUserId)
        {
            return await _dbSet
                .Where(p => p.AppUserId == appUserId)
                .Select(p => (int?)p.Id)
                .FirstOrDefaultAsync();
        }

        public async Task<Person?> GetPersonByAppUserIdAsync(string appUserId)
        {
            return await _dbSet
                .Include(p => p.AppUser)
                .FirstOrDefaultAsync(p => p.AppUserId == appUserId);
        }

        public async Task<string?> GetPersonFullNameAsync(int personId)
        {
            return await _dbSet
                .Where(p => p.Id == personId)
                .Select(p => p.AppUser.Fullname)
                .FirstOrDefaultAsync();
        }

        public async Task<string?> GetPersonEmailAsync(int personId)
        {
            return await _dbSet
                .Where(p => p.Id == personId)
                .Select(p => p.AppUser.Email)
                .FirstOrDefaultAsync();

        }

        public async Task<IEnumerable<Person>> GetWithInvitationsAndNotificationsAsync(IEnumerable<string> userIds)
        {
            return await _context.People
                .Include(p => p.Invitations)
                .Include(p => p.Notifications)
                .Include(p => p.AppUser)
                .Where(p => userIds.Contains(p.AppUserId))
                .ToListAsync();
        }

        public async Task<Person?> GetByAppUserIdWithInvitationsAndNotificationsAsync(string appUserId)
        {
            return await _context.People
                .Include(p => p.Invitations)
                .Include(p => p.Notifications)
                .Include(p => p.AppUser)
                .FirstOrDefaultAsync(p => p.AppUserId == appUserId);
        }

    }
}
