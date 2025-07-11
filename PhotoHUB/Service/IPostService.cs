using PhotoHUB.DTO;

namespace PhotoHUB.service;

public interface IPostService
{
    public Task<IEnumerable<PostPreviewDto>> GetPostsAsync(string token, int page, int pageSize);
    public Task<PostDto?> GetPostByIdAsync(string token, Guid postId);
    public Task<PostPreviewDto?> CreatePostAsync(string token, PostCreateDto postCreateDto);
    public Task<PostPreviewDto?> UpdatePostAsync(string token, PostUpdateDto postUpdateDto);
    public Task<bool> DeletePostAsync(string token, string postId);
    
}