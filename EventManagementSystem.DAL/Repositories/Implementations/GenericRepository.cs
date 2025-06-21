using EventManagementSystem.DAL.Data;
using EventManagementSystem.DAL.Entities.Common;
using EventManagementSystem.DAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace EventManagementSystem.DAL.Repositories.Implementations
{
    public class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : BaseEntity, new()
    {
        protected readonly EventManagementSystemDbContext _context;
        protected readonly DbSet<TEntity> _dbSet;

        public GenericRepository(EventManagementSystemDbContext context)
        {
            _context = context;
            _dbSet = _context.Set<TEntity>();
        }

        public async Task<TEntity?> GetByIdAsync(int id)
        {
            var model = await _dbSet.AsNoTracking().FirstOrDefaultAsync(m => m.Id == id);

            return model;
        }

        public async Task<IEnumerable<TEntity>> GetAllAsync()
        {
            var models = await _dbSet.AsNoTracking().ToListAsync();
            return models;
        }

        public async Task<TEntity> AddAsync(TEntity model)
        {
            await _dbSet.AddAsync(model);

            await _context.SaveChangesAsync();
            return model;
        }

        public async Task<TEntity> UpdateAsync(TEntity model)
        {
            model.UpdatedAt = DateTime.UtcNow.AddHours(4);

            _dbSet.Update(model);

            await _context.SaveChangesAsync();
            return model;

        }

        public async Task<bool> DeleteAsync(int id)
        {
            var model = await _dbSet.FirstOrDefaultAsync(m => m.Id == id);

            if (model == null)
            {
                return false;
            }

            model.IsDeleted = true;
            _dbSet.Update(model);

            await _context.SaveChangesAsync();
            return true;
        }
    }
}
