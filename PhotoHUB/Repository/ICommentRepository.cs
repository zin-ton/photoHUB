using PhotoHUB.models;

namespace PhotoHUB.Repository;

public interface ICommentRepository : IGenericRepository<Comment>
{
    Task<IEnumerable<Comment>> GetCommentsByPostIdAsync(Guid postId, int page, int pageSize);
    Task<IEnumerable<Comment>> GetRepliesByCommentIdAsync(Guid commentId, int page, int pageSize);
    Task<int> GetTotalCommentsCountByPostIdAsync(Guid postId);
    Task<int> GetTotalRepliesCountByCommentIdAsync(Guid commentId);
}