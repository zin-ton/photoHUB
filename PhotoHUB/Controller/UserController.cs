using Microsoft.AspNetCore.Mvc;
using PhotoHUB.service;

namespace PhotoHUB.controller;

[ApiController]
[Route("api/[controller]")]
public class UserController : ControllerBase
{
    private readonly IUserService _userService;
    private readonly ILogger<AuthController> _logger;
    public UserController(IUserService userService, ILogger<AuthController> logger)
    {
        _userService = userService;
        _logger = logger;
    }

    [HttpGet("profile")]
    public async Task<IActionResult> GetProfile()
    {
        string authHeader = Request.Headers["Authorization"];

        if (!string.IsNullOrEmpty(authHeader) && authHeader.StartsWith("Bearer "))
        {
            string token = authHeader.Substring("Bearer ".Length).Trim();
            try
            {
                var userInfo = await _userService.GetUserInfoAsync(token);
                if (userInfo != null)
                {
                    return Ok(userInfo);
                }
                else
                {
                    return NotFound(new { message = "User not found" });
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
    
    [HttpGet("savedPosts")]
    public async Task<IActionResult> GetSavedPosts()
    {
        string authHeader = Request.Headers["Authorization"];

        if (!string.IsNullOrEmpty(authHeader) && authHeader.StartsWith("Bearer "))
        {
            string token = authHeader.Substring("Bearer ".Length).Trim();
            try
            {
                var savedPosts = await _userService.GetSavedPostsAsync(token);
                return Ok(savedPosts);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving saved posts");
                return StatusCode(500, "Internal server error");
            }
        }
        else
        {
            return BadRequest("Invalid authorization header.");
        }
    }
    
    [HttpGet("likedPosts")]
    public async Task<IActionResult> GetLikedPosts()
    {
        string authHeader = Request.Headers["Authorization"];

        if (!string.IsNullOrEmpty(authHeader) && authHeader.StartsWith("Bearer "))
        {
            string token = authHeader.Substring("Bearer ".Length).Trim();
            try
            {
                var likedPosts = await _userService.GetLikedPostsAsync(token);
                return Ok(likedPosts);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving liked posts");
                return StatusCode(500, "Internal server error");
            }
        }
        else
        {
            return BadRequest("Invalid authorization header.");
        }
    }
    
    [HttpGet("myPosts")]
    public async Task<IActionResult> GetMyPosts()
    {
        string authHeader = Request.Headers["Authorization"];

        if (!string.IsNullOrEmpty(authHeader) && authHeader.StartsWith("Bearer "))
        {
            string token = authHeader.Substring("Bearer ".Length).Trim();
            try
            {
                var myPosts = await _userService.GetMyPostsAsync(token);
                return Ok(myPosts);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving user's posts");
                return StatusCode(500, "Internal server error");
            }
        }
        else
        {
            return BadRequest("Invalid authorization header.");
        }
    }
}