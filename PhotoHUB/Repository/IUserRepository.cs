using PhotoHUB.models;

namespace PhotoHUB.Repository;

public interface IUserRepository : IGenericRepository<User>
{
    Task<bool> LoginExistsAsync(string login);
    Task<bool> EmailExistsAsync(string email);
    Task<User?> GetByLoginAsync(string login);
    public Task<User?> GetByIdWithPostsAsync(Guid id);

}
