using Microsoft.AspNetCore.Authorization;
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

    [Authorize]
    [HttpGet("profile")]
    public async Task<IActionResult> GetProfile()
    {
        string? token = HttpContext.Request.Cookies["jwt_token"];
        if (string.IsNullOrEmpty(token))
        {
            return Unauthorized("No token provided");
        }

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

    [Authorize]
    [HttpGet("savedPosts")]
    public async Task<IActionResult> GetSavedPosts()
    {
        string? token = HttpContext.Request.Cookies["jwt_token"];
        if (string.IsNullOrEmpty(token))
        {
            return Unauthorized("No token provided");
        }

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

    [Authorize]
    [HttpGet("likedPosts")]
    public async Task<IActionResult> GetLikedPosts()
    {
        string? token = HttpContext.Request.Cookies["jwt_token"];
        if (string.IsNullOrEmpty(token))
        {
            return Unauthorized("No token provided");
        }

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

    [Authorize]
    [HttpGet("myPosts")]
    public async Task<IActionResult> GetMyPosts()
    {
        string? token = HttpContext.Request.Cookies["jwt_token"];
        if (string.IsNullOrEmpty(token))
        {
            return Unauthorized("No token provided");
        }

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
}