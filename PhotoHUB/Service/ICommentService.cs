using PhotoHUB.DTO;
using PhotoHUB.models;

namespace PhotoHUB.service;

public interface ICommentService
{
    public Task<CommentDto> CreateCommentAsync(string token, CreateCommentDto comment);
    public Task<CommentDto> UpdateCommentAsync(string token, UpdateCommentDto comment);
    public Task<bool> DeleteCommentAsync(string token, Guid commentId);
    public Task<IEnumerable<CommentDto>> GetCommentsByPostIdAsync(Guid postId, int page, int pageSize);
    public Task<IEnumerable<CommentDto>> GetRepliesByCommentIdAsync(Guid commentId, int page, int pageSize);
    public Task<int> GetTotalCommentsCountByPostIdAsync(Guid postId);
    public Task<int> GetTotalRepliesCountByCommentIdAsync(Guid commentId);
    public Task<CommentDto> GetCommentByIdAsync(Guid commentId);
    
}