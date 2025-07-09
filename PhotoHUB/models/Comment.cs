namespace PhotoHUB.models;

public class Comment
{
    public Guid Id { get; set; }

    public Guid PostId { get; set; }
    public Post Post { get; set; } = null!;

    public Guid UserId { get; set; }
    public User User { get; set; } = null!;

    public string Content { get; set; } = null!;
    public DateTime DateTime { get; set; }

    public Guid? ReplyToId { get; set; }
    public Comment? ReplyTo { get; set; }

    public ICollection<Comment> Replies { get; set; } = new List<Comment>();
}