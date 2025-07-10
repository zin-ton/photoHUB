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

    [HttpPost("create")]
    public async Task<ActionResult<Post>> Post([FromBody] PostCreateDTO post)
    {
        string authHeader = Request.Headers["Authorization"];

        if (!string.IsNullOrEmpty(authHeader) && authHeader.StartsWith("Bearer "))
        {
            string token = authHeader.Substring("Bearer ".Length).Trim();
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
        else
        {
            return BadRequest("Invalid authorization header.");
        }
    }
    
    [HttpGet("getPosts")]
    public async Task<ActionResult<IEnumerable<PostPreviewDTO>>> GetPosts(int page, int pageSize)
    {
        string authHeader = Request.Headers["Authorization"];

        if (!string.IsNullOrEmpty(authHeader) && authHeader.StartsWith("Bearer "))
        {
            string token = authHeader.Substring("Bearer ".Length).Trim();
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
        else
        {
            return BadRequest("Invalid authorization header.");
        }
    }
    
    [HttpGet("getPostById")]
    public async Task<ActionResult<PostPreviewDTO>> GetPostById([FromBody] string postId)
    {
        string authHeader = Request.Headers["Authorization"];

        if (!string.IsNullOrEmpty(authHeader) && authHeader.StartsWith("Bearer "))
        {
            string token = authHeader.Substring("Bearer ".Length).Trim();
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
        else
        {
            return BadRequest("Invalid authorization header.");
        }
    }
    
    [HttpPut("updatePost")]
    public async Task<ActionResult<PostPreviewDTO>> UpdatePost([FromBody] PostUpdateDTO postUpdateDto)
    {
        string authHeader = Request.Headers["Authorization"];

        if (!string.IsNullOrEmpty(authHeader) && authHeader.StartsWith("Bearer "))
        {
            string token = authHeader.Substring("Bearer ".Length).Trim();
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
        else
        {
            return BadRequest("Invalid authorization header.");
        }
    }
}