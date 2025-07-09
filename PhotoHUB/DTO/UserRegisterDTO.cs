using System.ComponentModel.DataAnnotations;
using PhotoHUB.validator;

namespace PhotoHUB.DTO;

public class UserRegisterDTO
{
    [Required]
    public string FirstName { get; set; } = null!;
    [Required]
    public string LastName { get; set; } = null!;
    [StrongPassword]
    public string Password { get; set; } = null!;
    [Required]
    public string Login { get; set; } = null!;
    [Required]
    [EmailAddress(ErrorMessage = "Invalid email format")]
    public string Email { get; set; } = null!;
}