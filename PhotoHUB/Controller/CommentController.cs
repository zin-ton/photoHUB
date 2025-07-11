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

    public CommentController(ICommentService commentService, ILogger<CommentController> logger)
    {
        _commentService = commentService;
    }
    
    [Authorize]
    [HttpPost("create")]
    public async Task<IActionResult> CreateCommentAsync([FromBody] CreateCommentDto comment)
    {
        string? token = HttpContext.Request.Cookies["jwt_token"];
        var result = await _commentService.CreateCommentAsync(token, comment);
        return Ok(result);
    }


    [Authorize]
    [HttpPut("update")]
    public async Task<IActionResult> UpdateCommentAsync([FromBody] UpdateCommentDto comment)
    {
        string? token = HttpContext.Request.Cookies["jwt_token"];
        var updatedComment = await _commentService.UpdateCommentAsync(token, comment);
        return Ok(updatedComment);
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
        return Ok(comments);
    }
    
    [Authorize]
    [HttpGet("replies/{commentId}")]
    public async Task<IActionResult> GetRepliesByCommentIdAsync(Guid commentId, int page, int pageSize)
    {
        var replies = await _commentService.GetRepliesByCommentIdAsync(commentId, page, pageSize);
        return Ok(replies);
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
        return Ok(comment);
    }
    
}