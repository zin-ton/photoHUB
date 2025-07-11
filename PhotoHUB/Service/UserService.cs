using AutoMapper;
using PhotoHUB.DTO;
using PhotoHUB.models;
using PhotoHUB.Repository;
using PhotoHUB.service;

namespace PhotoHUB.Service;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;
    private readonly IMapper _mapper;
    private readonly ILogger<UserService> _logger;
    private readonly JwtService _jwtService;
    public UserService(IUserRepository userRepository, IMapper mapper, ILogger<UserService> logger, JwtService jwtService)
    {
        _userRepository = userRepository;
        _mapper = mapper;
        _logger = logger;
        _jwtService = jwtService;
    }

    public User MapDtoToUser(UserRegisterDto dto)
    {
        var user = _mapper.Map<User>(dto);
        user.Password = PasswordHasher.HashPassword(dto.Password);
        return user;
    }

    public GetUserDto MapUserToGetUserDto(User user)
    {
        var userDto = _mapper.Map<GetUserDto>(user);
        return userDto;
    }

    public async Task<string> RegisterUserAsync(UserRegisterDto dto)
    {
        if (await _userRepository.LoginExistsAsync(dto.Login))
        {
            return "Login already exists.";
        }

        if (await _userRepository.EmailExistsAsync(dto.Email))
        {
            return "Email already exists.";
        }

        var user = _mapper.Map<User>(dto);
        user.Password = PasswordHasher.HashPassword(dto.Password);
        try
        {
            await _userRepository.AddAsync(user);
            return "User registered successfully";
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Registration failed");
            return "Registration failed";
        }
    }

    public async Task<string> LoginAsync(UserLoginDto dto)
    {
        var user = await _userRepository.GetByLoginAsync(dto.Username);
        if (user == null)
        {
            return "User not found";
        }

        if (!PasswordHasher.VerifyPassword(dto.Password, user.Password))
        {
            return "Invalid password";
        }

        return _jwtService.GenerateToken(user.Id.ToString(), user.Login);
    }

    public async Task<bool> VerifyPasswordAsync(string token, string password)
    {
        var userInfo = _jwtService.GetUserInfoFromToken(token);
        var user = await _userRepository.GetByIdAsync(userInfo.Guid);
        if (user == null) return false;

        return PasswordHasher.VerifyPassword(password, user.Password);
    }

    public async Task<GetUserDto?> GetUserInfoAsync(string token)
    {
        var userInfo = _jwtService.GetUserInfoFromToken(token);
        var user = await _userRepository.GetByIdAsync(userInfo.Guid);
        if (user == null)
        {
            _logger.LogWarning("User with ID {UserId} not found", userInfo.Guid);
            return null;
        }
        var userDto = _mapper.Map<GetUserDto>(user);
        return userDto;
    }
    
    public async Task<List<PostPreviewDto>> GetSavedPostsAsync(string token)
    {
        var userInfo = _jwtService.GetUserInfoFromToken(token);
        var user = await _userRepository.GetByIdAsync(userInfo.Guid);
        if (user == null)
        {
            _logger.LogWarning("User with ID {UserId} not found", userInfo.Guid);
            return new List<PostPreviewDto>();
        }
        
        var savedPosts = user.SavedPosts.Select(p => _mapper.Map<PostPreviewDto>(p)).ToList();
        return savedPosts;
    }
    
    public async Task<List<PostPreviewDto>> GetLikedPostsAsync(string token)
    {
        var userInfo = _jwtService.GetUserInfoFromToken(token);
        var user = await _userRepository.GetByIdAsync(userInfo.Guid);
        if (user == null)
        {
            _logger.LogWarning("User with ID {UserId} not found", userInfo.Guid);
            return new List<PostPreviewDto>();
        }
        
        var likedPosts = user.Likes.Select(l => _mapper.Map<PostPreviewDto>(l.Post)).ToList();
        return likedPosts;
    }
    
    public async Task<List<PostPreviewDto>> GetMyPostsAsync(string token)
    {
        var userInfo = _jwtService.GetUserInfoFromToken(token);
        var user = await _userRepository.GetByIdWithPostsAsync(userInfo.Guid);
        if (user == null)
        {
            _logger.LogWarning("User with ID {UserId} not found", userInfo.Guid);
            return new List<PostPreviewDto>();
        }

        List<PostPreviewDto> myPosts;
        if (user.Posts.Any())
        {
            myPosts = user.Posts.Select(p => _mapper.Map<PostPreviewDto>(p)).ToList();
        }
        else
        {
            _logger.LogWarning("User with ID {UserId} has no posts", userInfo.Guid);
            myPosts = new List<PostPreviewDto>();
        }
        return myPosts;
    }
}