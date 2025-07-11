using PhotoHUB.DTO;
using PhotoHUB.models;

namespace PhotoHUB.service;

public interface IUserService
{
    Task<string> RegisterUserAsync(UserRegisterDto user);
    public User MapDtoToUser(UserRegisterDto dto);
    Task<string> LoginAsync(UserLoginDto user);
    
    Task<bool> VerifyPasswordAsync(string token, string password);
    public GetUserDto MapUserToGetUserDto(User user);
    
    Task<GetUserDto?> GetUserInfoAsync(string token);
    
    Task<List<PostPreviewDto>> GetSavedPostsAsync(string token);
    
    Task<List<PostPreviewDto>> GetLikedPostsAsync(string token);
    
    Task<List<PostPreviewDto>> GetMyPostsAsync(string token);

}