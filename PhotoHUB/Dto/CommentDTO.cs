using PhotoHUB.models;

namespace PhotoHUB.DTO;

public class CommentDto
{
    public Guid Id { get; set; }
    public string Content { get; set; } = null!;
    public DateTime DateTime { get; set; }
    public Guid PostId { get; set; }
    public string AuthorLogin { get; set; } = null!;
    public Guid? ReplyToId { get; set; }
    public string AuthorS3Key { get; set; } = null!;
    public ICollection<Comment> Replies { get; set; } = new List<Comment>();

}