using Microsoft.EntityFrameworkCore;
using PhotoHUB.configs;
using PhotoHUB.models;

namespace PhotoHUB.Repository;

public class GenericRepository<T> : IGenericRepository<T> where T : Entity
{
    protected readonly PhotoHubContext _context;
    protected readonly DbSet<T> _dbSet;

    public GenericRepository(PhotoHubContext context)
    {
        _context = context;
        _dbSet = context.Set<T>();
    }
    
    public async Task<T?> GetByIdAsync(Guid id) => await _dbSet.FindAsync(id);

    public async Task<IEnumerable<T>> GetAllAsync() => await _dbSet.ToListAsync();

    public async Task<T> AddAsync(T entity)
    {
        await _dbSet.AddAsync(entity);
        await _context.SaveChangesAsync();
        return entity;
    }

    public async Task<T?> UpdateAsync(T entity)
    {
        _dbSet.Update(entity);
        await _context.SaveChangesAsync();
        return entity;
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        var entity = await _dbSet.FindAsync(id);
        if (entity == null) return false;
        _dbSet.Remove(entity);
        return await _context.SaveChangesAsync() > 0;
    }
}