using Microsoft.EntityFrameworkCore;
using PhotoHUB.configs;
using PhotoHUB.models;

namespace PhotoHUB.Repository;

public class PostRepository : GenericRepository<Post>, IPostRepository
{
    private readonly PhotoHubContext _context;

    public PostRepository(PhotoHubContext context) : base(context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Post>> GetPostAsync(int page, int pageSize)
    {
        var posts = _context.Posts
            .Include(p => p.User)
            .Include(p => p.Comments)
            .OrderByDescending(p => p.DateTime)
            .Skip((page - 1) * pageSize)
            .Take(pageSize);
        return await posts.ToListAsync();
    }
}