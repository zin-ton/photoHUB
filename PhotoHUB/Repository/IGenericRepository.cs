using PhotoHUB.models;

namespace PhotoHUB.Repository;

public interface IGenericRepository<T> where T : Entity
{
    public Task<T?> GetByIdAsync(Guid id);
    public Task<IEnumerable<T>> GetAllAsync();
    public Task<T> AddAsync(T entity);
    public Task<T?> UpdateAsync(T entity);
    public Task<bool> DeleteAsync(Guid id);
}