using PhotoHUB.models;

namespace PhotoHUB.Repository;

public interface IUserRepository
{
    Task<bool> LoginExistsAsync(string login);
    Task<bool> EmailExistsAsync(string email);
    Task AddUserAsync(User user);
    Task<User?> GetByLoginAsync(string login);
    Task<User?> GetByIdAsync(Guid id);
    Task SaveChangesAsync();
}
