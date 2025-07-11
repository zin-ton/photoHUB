namespace PhotoHUB.DTO;

public class CreateCommentDTO
{
    public Guid PostId { get; set; }
    public string Content { get; set; } = null!;
    public string? ReplyToId { get; set; }
}