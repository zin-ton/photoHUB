using Microsoft.EntityFrameworkCore;
using PhotoHUB.configs;
using PhotoHUB.models;

namespace PhotoHUB.Repository;

public class UserRepository : GenericRepository<User>, IUserRepository 
{
    private readonly PhotoHubContext _context;
    public UserRepository(PhotoHubContext context) : base(context)
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

    public async Task<User?> GetByLoginAsync(string login)
    {
        return await _context.Users.FirstOrDefaultAsync(u => u.Login == login);
    }
    
    public async Task<User?> GetByIdWithPostsAsync(Guid id)
    {
        return await _context.Users
            .Include(u => u.Posts)
            .FirstOrDefaultAsync(u => u.Id == id);
    }
    
}
