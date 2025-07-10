using PhotoHUB.models;

namespace PhotoHUB.DTO;

public class CommentDTO
{
    public string Content { get; set; } = null!;
    public DateTime DateTime { get; set; }
    public ICollection<Comment> Replies { get; set; } = new List<Comment>();

}