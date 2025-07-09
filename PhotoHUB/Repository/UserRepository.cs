using Microsoft.EntityFrameworkCore;
using PhotoHUB.configs;
using PhotoHUB.models;

namespace PhotoHUB.Repository;

public class UserRepository : IUserRepository
{
    private readonly PhotoHubContext _context;

    public UserRepository(PhotoHubContext context)
    {
        _context = context;
    }

    public async Task<bool> LoginExistsAsync(string login)
    {
        return await _context.Users.AnyAsync(u => u.Login == login);
    }

    public async Task<bool> EmailExistsAsync(string email)
    {
        return await _context.Users.AnyAsync(u => u.Email == email);
    }

    public async Task AddUserAsync(User user)
    {
        await _context.Users.AddAsync(user);
    }

    public async Task<User?> GetByLoginAsync(string login)
    {
        return await _context.Users.FirstOrDefaultAsync(u => u.Login == login);
    }

    public async Task<User?> GetByIdAsync(Guid id)
    {
        return await _context.Users.FindAsync(id);
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }
}
