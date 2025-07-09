using AutoMapper;
using PhotoHUB.DTO;
using PhotoHUB.models;

namespace PhotoHUB.service;

public interface IUserService
{
    Task<string> RegisterUserAsync(UserRegisterDTO user);
    public User MapDtoToUser(UserRegisterDTO dto);
    Task<string> LoginAsync(UserLoginDTO user);
    
    Task<bool> VerifyPasswordAsync(Guid userId, string password);
    public GetUserDTO MapUserToGetUserDTO(User user);
    
    Task<GetUserDTO?> GetUserInfoAsync(Guid userId);

}