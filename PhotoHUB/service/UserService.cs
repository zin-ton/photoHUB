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

    public UserService(IUserRepository userRepository, IMapper mapper, ILogger<UserService> logger)
    {
        _userRepository = userRepository;
        _mapper = mapper;
        _logger = logger;
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

        await _userRepository.AddUserAsync(user);

        try
        {
            await _userRepository.SaveChangesAsync();
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
        var user = await _userRepository.GetByLoginAsync(dto.username);
        if (user == null)
        {
            return "User not found";
        }

        if (!PasswordHasher.VerifyPassword(dto.password, user.Password))
        {
            return "Invalid password";
        }

        return "Login successful";
    }

    public async Task<bool> VerifyPasswordAsync(Guid userId, string password)
    {
        var user = await _userRepository.GetByIdAsync(userId);
        if (user == null) return false;

        return PasswordHasher.VerifyPassword(password, user.Password);
    }

    public async Task<GetUserDTO?> GetUserInfoAsync(Guid userId)
    {
        var user = await _userRepository.GetByIdAsync(userId);
        if (user == null)
        {
            _logger.LogWarning("User with ID {UserId} not found", userId);
            return null;
        }
        var userDto = _mapper.Map<GetUserDTO>(user);
        return userDto;
    }
}