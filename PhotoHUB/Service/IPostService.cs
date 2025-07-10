using PhotoHUB.DTO;

namespace PhotoHUB.service;

public interface IPostService
{
    public Task<IEnumerable<PostPreviewDTO>> GetPostsAsync(string token, int page, int pageSize);
    public Task<PostDTO?> GetPostByIdAsync(string token, Guid postId);
    public Task<PostPreviewDTO?> CreatePostAsync(string token, PostCreateDTO postCreateDto);
    public Task<PostPreviewDTO?> UpdatePostAsync(string token, PostUpdateDTO postUpdateDto);
    public Task<bool> DeletePostAsync(string token, string postId);
    
}