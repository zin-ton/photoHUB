using System.ComponentModel.DataAnnotations;
using PhotoHUB.validator;

namespace PhotoHUB.DTO;

public class UserLoginDTO
{
    [Required]
    public string Username{ get; set; } = null!;
    [StrongPassword]
    public string Password{ get; set; } = null!;
}