using Microsoft.EntityFrameworkCore;
using PhotoHUB.configs;
using PhotoHUB.models;

namespace PhotoHUB.Repository;

public class CommentRepository : GenericRepository<Comment>, ICommentRepository
{
    private readonly PhotoHubContext _context;
    public CommentRepository(PhotoHubContext context) : base(context)
    {
        _context = context;
    }
    
    public async Task<IEnumerable<Comment>> GetCommentsByPostIdAsync(Guid postId, int page, int pageSize)
    {
        return await _context.Comments
            .Where(c => c.PostId == postId && c.ReplyTo == null)
            .OrderByDescending(c => c.DateTime)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Include(c => c.User)
            .ToListAsync();
    }
    
    public async Task<IEnumerable<Comment>> GetRepliesByCommentIdAsync(Guid commentId, int page, int pageSize)
    {
        return await _context.Comments
            .Where(c => c.ReplyToId == commentId)
            .OrderByDescending(c => c.DateTime)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Include(c => c.User)
            .ToListAsync();
    }
    
    public async Task<int> GetTotalCommentsCountByPostIdAsync(Guid postId)
    {
        return await _context.Comments
            .CountAsync(c => c.PostId == postId && c.ReplyTo == null);
    }
    
    public async Task<int> GetTotalRepliesCountByCommentIdAsync(Guid commentId)
    {
        return await _context.Comments
            .CountAsync(c => c.ReplyToId == commentId);
    }
}