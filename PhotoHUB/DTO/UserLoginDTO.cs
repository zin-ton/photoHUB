using System.ComponentModel.DataAnnotations;
using PhotoHUB.validator;

namespace PhotoHUB.DTO;

public class UserLoginDTO
{
    [Required]
    public string username{ get; set; } = null!;
    [StrongPassword]
    public string password{ get; set; } = null!;
}