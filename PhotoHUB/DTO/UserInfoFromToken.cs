namespace PhotoHUB.DTO;

public class UserInfoFromToken
{
    public Guid Guid { get; set; }
    public string Login { get; set; } = null!;
}