using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PhotoHUB.DTO;
using PhotoHUB.service;

namespace PhotoHUB.controller;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IUserService _userService;
    private readonly ILogger<AuthController> _logger;
    public AuthController(IUserService userService, ILogger<AuthController> logger)
    {
        _userService = userService;
        _logger = logger;
    }
    
    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] UserRegisterDTO registrationDto)
    {
        if (registrationDto == null)
        {
            return BadRequest("Invalid registration data.");
        }

        try
        {
            var result = await _userService.RegisterUserAsync(registrationDto);
            if (result == "User registered successfully")
            {
                return Ok(new { message = result });
            }
            else
            {
                return BadRequest(new { message = result });
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Registration failed");
            return StatusCode(500, "Internal server error");
        }
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] UserLoginDTO loginDto)
    {
        if(loginDto == null)
            return BadRequest("Invalid login data.");
        try
        {
            var result = await _userService.LoginAsync(loginDto);
            if (result == "Invalid password" || result == "User not found")
            {
                return Unauthorized(new { message = result });
            }
            else
            {
                Response.Cookies.Append("jwt_token", result, new CookieOptions
                {
                    HttpOnly = true,
                    Secure = true, 
                    SameSite = SameSiteMode.Strict,
                    Expires = DateTimeOffset.UtcNow.AddHours(1)
                });
                return Ok(new { message = result });
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Login failed");
            return StatusCode(500, "Internal server error");
        }
    }
    
    [Authorize]
    [HttpPost("verifyPassword")]
    public async Task<IActionResult> VerifyPassword([FromBody] string password)
    {
            string? token = HttpContext.Request.Cookies["jwt_token"];
            if(token == null) 
                return Unauthorized(new { message = "Unauthorized access. Token is missing." });
            try
            {
                bool isValid = await _userService.VerifyPasswordAsync(token, password);
                if (isValid)
                {
                    return Ok(new { message = "Password is valid." });
                }
                else
                {
                    return Unauthorized(new { message = "Invalid password." });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Password verification failed");
                return StatusCode(500, "Internal server error");
            }
    }
}