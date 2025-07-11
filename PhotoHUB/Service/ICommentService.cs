using PhotoHUB.DTO;
using PhotoHUB.models;

namespace PhotoHUB.service;

public interface ICommentService
{
    public Task<CommentDTO> CreateCommentAsync(string token, CreateCommentDTO comment);
    public Task<CommentDTO> UpdateCommentAsync(string token, UpdateCommentDTO comment);
    public Task<bool> DeleteCommentAsync(string token, Guid commentId);
    public Task<IEnumerable<CommentDTO>> GetCommentsByPostIdAsync(Guid postId, int page, int pageSize);
    public Task<IEnumerable<CommentDTO>> GetRepliesByCommentIdAsync(Guid commentId, int page, int pageSize);
    public Task<int> GetTotalCommentsCountByPostIdAsync(Guid postId);
    public Task<int> GetTotalRepliesCountByCommentIdAsync(Guid commentId);
    public Task<CommentDTO> GetCommentByIdAsync(Guid commentId);
    
}