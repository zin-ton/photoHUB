using PhotoHUB.DTO;
using PhotoHUB.models;

namespace PhotoHUB.service;

public interface IUserService
{
    Task<string> RegisterUserAsync(UserRegisterDTO user);
    public User MapDtoToUser(UserRegisterDTO dto);
    Task<string> LoginAsync(UserLoginDTO user);
    
    Task<bool> VerifyPasswordAsync(string token, string password);
    public GetUserDTO MapUserToGetUserDTO(User user);
    
    Task<GetUserDTO?> GetUserInfoAsync(string token);
    
    Task<List<PostPreviewDTO>> GetSavedPostsAsync(string token);
    
    Task<List<PostPreviewDTO>> GetLikedPostsAsync(string token);
    
    Task<List<PostPreviewDTO>> GetMyPostsAsync(string token);

}