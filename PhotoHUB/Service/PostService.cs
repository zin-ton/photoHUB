using AutoMapper;
using PhotoHUB.DTO;
using PhotoHUB.models;
using PhotoHUB.Repository;

namespace PhotoHUB.service;

public class PostService : IPostService
{
    private readonly IUserRepository _userRepository;
    private readonly IPostRepository _postRepository;
    private readonly IMapper _mapper;
    private readonly ILogger<UserService> _logger;
    private readonly JwtService _jwtService;
    
    public PostService(IPostRepository postRepository, IMapper mapper, ILogger<UserService> logger, JwtService jwtService, 
        IUserRepository userRepository)
    {
        _postRepository = postRepository;
        _mapper = mapper;
        _logger = logger;
        _jwtService = jwtService;
        _userRepository = userRepository;
    }
    
    public async Task<IEnumerable<PostPreviewDTO>> GetPostsAsync(string token, int page, int pageSize)
    {
        var userInfo = _jwtService.GetUserInfoFromToken(token);
        var user = await _userRepository.GetByIdAsync(userInfo.Guid);
        if (user == null)
        {
            _logger.LogWarning("User with ID {UserId} not found", userInfo.Guid);
            return null;
        }
        var posts = await _postRepository.GetPostAsync(page, pageSize);
        return _mapper.Map<IEnumerable<PostPreviewDTO>>(posts);
    }
    
    public async Task<PostDTO?> GetPostByIdAsync(string token, Guid postId)
    {
        var userInfo = _jwtService.GetUserInfoFromToken(token);
        var user = await _userRepository.GetByIdAsync(userInfo.Guid);
        if (user == null)
        {
            _logger.LogWarning("User with ID {UserId} not found", userInfo.Guid);
            return null;
        }
        
        var post = await _postRepository.GetByIdAsync(postId);
        if (post == null)
        {
            _logger.LogWarning("Post with ID {PostId} not found", postId);
            return null;
        }
        
        PostDTO postDTO = _mapper.Map<PostDTO>(post);
        postDTO.Comments = _mapper.Map<ICollection<CommentDTO>>(post.Comments);
        return postDTO;
    }
    
    public async Task<PostPreviewDTO?> CreatePostAsync(string token, PostCreateDTO postCreateDto) //TODO add geting post from db before returning and change PostPreviewDTO to store username and categories
    {
        var userInfo = _jwtService.GetUserInfoFromToken(token);
        var user = await _userRepository.GetByIdAsync(userInfo.Guid);
        if (user == null)
        {
            _logger.LogWarning("User with ID {UserId} not found", userInfo.Guid);
            return null;
        }
        
        var post = _mapper.Map<Post>(postCreateDto);
        post.UserId = user.Id;
        post.DateTime = DateTime.UtcNow;
        var createdPost = await _postRepository.AddAsync(post);
        if (createdPost == null)
        {
            _logger.LogError("Failed to create post for user with ID {UserId}", userInfo.Guid);
            return null;
        }
        
        return _mapper.Map<PostPreviewDTO>(createdPost);
    }
    
    public async Task<PostPreviewDTO?> UpdatePostAsync(string token, PostUpdateDTO postUpdateDto)
    {
        var userInfo = _jwtService.GetUserInfoFromToken(token);
        var user = await _userRepository.GetByIdAsync(userInfo.Guid);
        if (user == null)
        {
            _logger.LogWarning("User with ID {UserId} not found", userInfo.Guid);
            return null;
        }
        
        var post = await _postRepository.GetByIdAsync(postUpdateDto.Id);
        if (post == null || post.User != user)
        {
            _logger.LogWarning("Post with ID {PostId} not found or does not belong to user with ID {UserId}", postUpdateDto.Id, userInfo.Guid);
            return null;
        }
        
        _mapper.Map(postUpdateDto, post);
        post.DateTime = DateTime.SpecifyKind(post.DateTime, DateTimeKind.Utc);
        var updatedPost = await _postRepository.UpdateAsync(post);
        
        return _mapper.Map<PostPreviewDTO>(updatedPost);
    }
    
    public async Task<bool> DeletePostAsync(string token, string postId)
    {
        var userInfo = _jwtService.GetUserInfoFromToken(token);
        var user = await _userRepository.GetByIdAsync(userInfo.Guid);
        if (user == null)
        {
            _logger.LogWarning("User with ID {UserId} not found", userInfo.Guid);
            return false;
        }
        
        var post = await _postRepository.GetByIdAsync(Guid.Parse(postId));
        if (post == null || post.User != user)
        {
            _logger.LogWarning("Post with ID {PostId} not found or does not belong to user with ID {UserId}", postId, userInfo.Guid);
            return false;
        }
        
        return await _postRepository.DeleteAsync(post.Id);
    }
    
    
    
}