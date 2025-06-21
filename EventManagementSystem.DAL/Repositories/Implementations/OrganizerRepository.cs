using EventManagementSystem.DAL.Data;
using EventManagementSystem.DAL.Entities;
using EventManagementSystem.DAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace EventManagementSystem.DAL.Repositories.Implementations
{
    public class OrganizerRepository : GenericRepository<Organizer>, IOrganizerRepository
    {
        public OrganizerRepository(EventManagementSystemDbContext context) : base(context)
        {
        }
        public async Task<Organizer?> GetOrganizerByIdWithAppUserAsync(int id)
        {
            return await _dbSet
                .Include(o => o.AppUser)
                .FirstOrDefaultAsync(o => o.Id == id);
        }
        public async Task<IEnumerable<Organizer>> GetAllOrganizersWithAppUserAsync()
        {
            return await _dbSet
                .Include(o => o.AppUser)
                .ToListAsync();
        }
        public async Task<Organizer?> GetOrganizerByAppUserIdAsync(string appUserId)
        {
            return await _dbSet
                .Include(o => o.AppUser)
                .FirstOrDefaultAsync(o => o.AppUserId == appUserId);
        }
        public async Task<IEnumerable<Organizer>> SearchAsync(string searchTerm)
        {
            if (string.IsNullOrWhiteSpace(searchTerm))
            {
                throw new ArgumentException("Search term cannot be null or empty.", nameof(searchTerm));
            }
            return await _dbSet
                .Include(o => o.AppUser)
                .Where(o => o.AppUser.Name.Contains(searchTerm) || o.AppUser.UserName.Contains(searchTerm))
                .ToListAsync();
        }
    }
}
