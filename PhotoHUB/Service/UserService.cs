using AutoMapper;
using PhotoHUB.DTO;
using PhotoHUB.models;
using PhotoHUB.Repository;
using PhotoHUB.service;

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

    public User MapDtoToUser(UserRegisterDTO dto)
    {
        var user = _mapper.Map<User>(dto);
        user.Password = PasswordHasher.HashPassword(dto.Password);
        return user;
    }

    public GetUserDTO MapUserToGetUserDTO(User user)
    {
        var userDTO = _mapper.Map<GetUserDTO>(user);
        return userDTO;
    }

    public async Task<string> RegisterUserAsync(UserRegisterDTO dto)
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

    public async Task<string> LoginAsync(UserLoginDTO dto)
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

    public async Task<GetUserDTO?> GetUserInfoAsync(string token)
    {
        var userInfo = _jwtService.GetUserInfoFromToken(token);
        var user = await _userRepository.GetByIdAsync(userInfo.Guid);
        if (user == null)
        {
            _logger.LogWarning("User with ID {UserId} not found", userInfo.Guid);
            return null;
        }
        var userDto = _mapper.Map<GetUserDTO>(user);
        return userDto;
    }
    
    public async Task<List<PostPreviewDTO>> GetSavedPostsAsync(string token)
    {
        var userInfo = _jwtService.GetUserInfoFromToken(token);
        var user = await _userRepository.GetByIdAsync(userInfo.Guid);
        if (user == null)
        {
            _logger.LogWarning("User with ID {UserId} not found", userInfo.Guid);
            return new List<PostPreviewDTO>();
        }
        
        var savedPosts = user.SavedPosts.Select(p => _mapper.Map<PostPreviewDTO>(p)).ToList();
        return savedPosts;
    }
    
    public async Task<List<PostPreviewDTO>> GetLikedPostsAsync(string token)
    {
        var userInfo = _jwtService.GetUserInfoFromToken(token);
        var user = await _userRepository.GetByIdAsync(userInfo.Guid);
        if (user == null)
        {
            _logger.LogWarning("User with ID {UserId} not found", userInfo.Guid);
            return new List<PostPreviewDTO>();
        }
        
        var likedPosts = user.Likes.Select(l => _mapper.Map<PostPreviewDTO>(l.Post)).ToList();
        return likedPosts;
    }
    
    public async Task<List<PostPreviewDTO>> GetMyPostsAsync(string token)
    {
        var userInfo = _jwtService.GetUserInfoFromToken(token);
        var user = await _userRepository.GetByIdWithPostsAsync(userInfo.Guid);
        if (user == null)
        {
            _logger.LogWarning("User with ID {UserId} not found", userInfo.Guid);
            return new List<PostPreviewDTO>();
        }

        var myPosts = new List<PostPreviewDTO>();
        if (user.Posts != null && user.Posts.Any())
        {
            myPosts = user.Posts.Select(p => _mapper.Map<PostPreviewDTO>(p)).ToList();
        }
        else
        {
            _logger.LogWarning("User with ID {UserId} has no posts", userInfo.Guid);
            myPosts = new List<PostPreviewDTO>();
        }
        return myPosts;
    }
}