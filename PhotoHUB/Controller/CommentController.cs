using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PhotoHUB.DTO;
using PhotoHUB.service;

namespace PhotoHUB.controller;

[ApiController]
[Route("api/[controller]")]
public class CommentController : ControllerBase
{
    private readonly ICommentService _commentService;
    private readonly ILogger<CommentController> _logger;
    
    public CommentController(ICommentService commentService, ILogger<CommentController> logger)
    {
        _commentService = commentService;
        _logger = logger;
    }
    
    [Authorize]
    [HttpPost("create")]
    public async Task<IActionResult> CreateCommentAsync([FromBody] CreateCommentDTO comment)
    {
        string? token = HttpContext.Request.Cookies["jwt_token"];
        var result = await _commentService.CreateCommentAsync(token, comment);
        if (result != null)
        {
            return Ok(result);
        }
        else
        {
            return StatusCode(500, "Internal server error while creating comment");
        }
    }


    [Authorize]
    [HttpPut("update")]
    public async Task<IActionResult> UpdateCommentAsync([FromBody] UpdateCommentDTO comment)
    {
        string? token = HttpContext.Request.Cookies["jwt_token"];
        var updatedComment = await _commentService.UpdateCommentAsync(token, comment);
        if (updatedComment != null)
        {
            return Ok(updatedComment);
        }
        else
        {
            return NotFound("Comment not found or update failed");
        }
    }
    
    [Authorize]
    [HttpDelete("delete/{commentId}")]
    public async Task<IActionResult> DeleteCommentAsync(Guid commentId)
    {
        string? token = HttpContext.Request.Cookies["jwt_token"];
        if (await _commentService.DeleteCommentAsync(token, commentId))
        {
            return Ok("Comment deleted successfully");
        }
        else
        {
            return NotFound("Comment not found or deletion failed");
        }
    }
    
    [Authorize]
    [HttpGet("post/{postId}")]
    public async Task<IActionResult> GetCommentsByPostIdAsync(Guid postId, int page, int pageSize)
    {
        var comments = await _commentService.GetCommentsByPostIdAsync(postId, page, pageSize);
        if (comments != null)
        {
            return Ok(comments);
        }
        else
        {
            return NotFound("No comments found for this post");
        }
    }
    
    [Authorize]
    [HttpGet("replies/{commentId}")]
    public async Task<IActionResult> GetRepliesByCommentIdAsync(Guid commentId, int page, int pageSize)
    {
        var replies = await _commentService.GetRepliesByCommentIdAsync(commentId, page, pageSize);
        if (replies != null)
        {
            return Ok(replies);
        }
        else
        {
            return NotFound("No replies found for this comment");
        }
    }
    
    [Authorize]
    [HttpGet("count/post/{postId}")]
    public async Task<IActionResult> GetTotalCommentsCountByPostIdAsync(Guid postId)
    {
        var count = await _commentService.GetTotalCommentsCountByPostIdAsync(postId);
        return Ok(new { TotalComments = count });
    }
    
    [Authorize]
    [HttpGet("count/replies/{commentId}")]
    public async Task<IActionResult> GetTotalRepliesCountByCommentIdAsync(Guid commentId)
    {
        var count = await _commentService.GetTotalRepliesCountByCommentIdAsync(commentId);
        return Ok(new { TotalReplies = count });
    }
    
    [Authorize]
    [HttpGet("{commentId}")]
    public async Task<IActionResult> GetCommentByIdAsync(Guid commentId)
    {
        var comment = await _commentService.GetCommentByIdAsync(commentId);
        if (comment != null)
        {
            return Ok(comment);
        }
        else
        {
            return NotFound("Comment not found");
        }
    }
    
}