using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PhotoHUB.DTO;
using PhotoHUB.models;
using PhotoHUB.service;

namespace PhotoHUB.controller;

[ApiController]
[Route("api/[controller]")]
public class PostController : ControllerBase
{
    private readonly IPostService _postService;
    private readonly ILogger<PostController> _logger;

    public PostController(IPostService postService, ILogger<PostController> logger)
    {
        _postService = postService;
        _logger = logger;
    }

    [Authorize]
    [HttpPost("create")]
    public async Task<ActionResult<Post>> Post([FromBody] PostCreateDTO post)
    {
        string? token = HttpContext.Request.Cookies["jwt_token"];
        if (string.IsNullOrEmpty(token))
        {
            return Unauthorized("No token provided");
        }

        try
        {
            var CreatedPost = await _postService.CreatePostAsync(token, post);
            if (CreatedPost != null)
            {
                return Ok(CreatedPost);
            }
            else
            {
                return StatusCode(500, "Internal server error while creating post");
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving user profile");
            return StatusCode(500, "Internal server error");
        }
    }

    [Authorize]
    [HttpGet("getPosts")]
    public async Task<ActionResult<IEnumerable<PostPreviewDTO>>> GetPosts(int page, int pageSize)
    {
        string? token = HttpContext.Request.Cookies["jwt_token"];
        if (string.IsNullOrEmpty(token))
        {
            return Unauthorized("No token provided");
        }

        try
        {
            var posts = await _postService.GetPostsAsync(token, page, pageSize);
            if (posts != null)
            {
                return Ok(posts);
            }
            else
            {
                return NotFound(new { message = "No posts found" });
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving posts");
            return StatusCode(500, "Internal server error");
        }
    }

    [Authorize]
    [HttpGet("getPostById")]
    public async Task<ActionResult<PostPreviewDTO>> GetPostById(string postId)
    {
        string? token = HttpContext.Request.Cookies["jwt_token"];
        if (string.IsNullOrEmpty(token))
        {
            return Unauthorized("No token provided");
        }

        try
        {
            var post = await _postService.GetPostByIdAsync(token, Guid.Parse(postId));
            if (post != null)
            {
                return Ok(post);
            }
            else
            {
                return NotFound(new { message = "Post not found" });
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving post by ID");
            return StatusCode(500, "Internal server error");
        }
    }

    [Authorize]
    [HttpPut("updatePost")]
    public async Task<ActionResult<PostPreviewDTO>> UpdatePost([FromBody] PostUpdateDTO postUpdateDto)
    {
        string? token = HttpContext.Request.Cookies["jwt_token"];
        if (string.IsNullOrEmpty(token))
        {
            return Unauthorized("No token provided");
        }

        try
        {
            var updatedPost = await _postService.UpdatePostAsync(token, postUpdateDto);
            if (updatedPost != null)
            {
                return Ok(updatedPost);
            }
            else
            {
                return NotFound(new { message = "Post not found or update failed" });
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating post");
            return StatusCode(500, "Internal server error");
        }
    }

    [Authorize]
    [HttpDelete("deletePost")]
    public async Task<ActionResult<bool>> DeletePost(string postId)
    {
        string? token = HttpContext.Request.Cookies["jwt_token"];
        if (string.IsNullOrEmpty(token))
        {
            return Unauthorized("No token provided");
        }

        try
        {
            var result = await _postService.DeletePostAsync(token, postId);
            if (result)
            {
                return Ok(new { message = "Post deleted successfully" });
            }
            else
            {
                return NotFound(new { message = "Post not found or deletion failed" });
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting post");
            return StatusCode(500, "Internal server error");
        }
    }
}