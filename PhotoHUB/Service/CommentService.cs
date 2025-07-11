using AutoMapper;
using PhotoHUB.DTO;
using PhotoHUB.models;
using PhotoHUB.Repository;
using PhotoHUB.Service;

namespace PhotoHUB.service;

public class CommentService : ICommentService
{
    private readonly IUserRepository _userRepository;
    private readonly IMapper _mapper;
    private readonly ICommentRepository _commentRepository;
    private readonly ILogger<UserService> _logger;
    private readonly JwtService _jwtService;
    
    public CommentService(ICommentRepository commentRepository, IMapper mapper, ILogger<UserService> logger, JwtService jwtService, 
        IUserRepository userRepository)
    {
        _commentRepository = commentRepository;
        _mapper = mapper;
        _logger = logger;
        _jwtService = jwtService;
        _userRepository = userRepository;
    }
    
    public async Task<CommentDto> CreateCommentAsync(string token, CreateCommentDto comment)
    {
        var userInfo = _jwtService.GetUserInfoFromToken(token);
        var user = await _userRepository.GetByIdAsync(userInfo.Guid);
        if (user == null)
        {
            _logger.LogWarning("User with ID {UserId} not found", userInfo.Guid);
            return null!;
        }
        Comment newComment = _mapper.Map<Comment>(comment);
        newComment.UserId = user.Id;
        newComment.DateTime = DateTime.UtcNow;
        return _mapper.Map<CommentDto>(await _commentRepository.AddAsync(newComment));
    }
    
    public async Task<IEnumerable<CommentDto>> GetCommentsByPostIdAsync(Guid postId, int page, int pageSize)
    {
        var comments = await _commentRepository.GetCommentsByPostIdAsync(postId, page, pageSize);
        return _mapper.Map<IEnumerable<CommentDto>>(comments);
    }
    
    public async Task<IEnumerable<CommentDto>> GetRepliesByCommentIdAsync(Guid commentId, int page, int pageSize)
    {
        var replies = await _commentRepository.GetRepliesByCommentIdAsync(commentId, page, pageSize);
        return _mapper.Map<IEnumerable<CommentDto>>(replies);
    }
    
    public async Task<int> GetTotalCommentsCountByPostIdAsync(Guid postId)
    {
        return await _commentRepository.GetTotalCommentsCountByPostIdAsync(postId);
    }
    
    public async Task<int> GetTotalRepliesCountByCommentIdAsync(Guid commentId)
    {
        return await _commentRepository.GetTotalRepliesCountByCommentIdAsync(commentId);
    }
    
    public async Task<CommentDto> UpdateCommentAsync(string token, UpdateCommentDto comment)
    {
        var userInfo = _jwtService.GetUserInfoFromToken(token);
        var user = await _userRepository.GetByIdAsync(userInfo.Guid);
        if (user == null)
        {
            _logger.LogWarning("User with ID {UserId} not found", userInfo.Guid);
            return null!;
        }
        var updateComment = await _commentRepository.GetByIdAsync(comment.Id);
        if(user != updateComment!.User) 
        {
            _logger.LogWarning("User with ID {UserId} is not authorized to update this comment", userInfo.Guid);
            return null!;
        }
        updateComment.Content = comment.Content;
        updateComment.DateTime = DateTime.SpecifyKind(updateComment.DateTime, DateTimeKind.Utc);
        return _mapper.Map<CommentDto>(await _commentRepository.UpdateAsync(updateComment));
    }
    
    public async Task<bool> DeleteCommentAsync(string token, Guid commentId)
    {
        var userInfo = _jwtService.GetUserInfoFromToken(token);
        var user = await _userRepository.GetByIdAsync(userInfo.Guid);
        if (user == null)
        {
            _logger.LogWarning("User with ID {UserId} not found", userInfo.Guid);
            return false;
        }
        
        var comment = await _commentRepository.GetByIdAsync(commentId);
        if (comment == null)
        {
            _logger.LogWarning("Comment with ID {CommentId} not found", commentId);
            return false;
        }
        
        if (comment.User != user)
        {
            _logger.LogWarning("User with ID {UserId} is not authorized to delete this comment", userInfo.Guid);
            return false;
        }

        comment.UserId = Guid.Parse("4d374c58-ee4c-4d17-89d6-dbc789e207e5");
        comment.Content = "This comment has been deleted";
        comment.DateTime = DateTime.SpecifyKind(comment.DateTime, DateTimeKind.Utc);
        if (await _commentRepository.UpdateAsync(comment) != null) return true;
        else return false;

    }
    
    public async Task<CommentDto> GetCommentByIdAsync(Guid commentId)
    {
        return _mapper.Map<CommentDto>(await _commentRepository.GetByIdAsync(commentId));
    }
    
    
}