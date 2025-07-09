namespace PhotoHUB.models;

public class Like : Entity
{

    public Guid PostId { get; set; }
    public Post Post { get; set; } = null!;

    public Guid UserId { get; set; }
    public User User { get; set; } = null!;

    public DateTime DateTime { get; set; }
}