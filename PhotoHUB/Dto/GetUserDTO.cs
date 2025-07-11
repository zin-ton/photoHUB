using PhotoHUB.models;

namespace PhotoHUB.DTO;

public class GetUserDto
{
    public Guid Id { get; set; }
    public string Login { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
}