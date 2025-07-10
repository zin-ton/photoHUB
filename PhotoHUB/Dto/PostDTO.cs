namespace PhotoHUB.DTO;

public class PostDTO
{
    public Guid Guid { get; set; }
    public string Name { get; set; } = null!;
    public string Description { get; set; } = null!;
    public string S3Key { get; set; } = null!;
    public DateTime DateTime { get; set; }
    public ICollection<CommentDTO> Comments { get; set; } = new List<CommentDTO>();
    public int Likes { get; set; }
    public string AuthorLogin { get; set; } = null!;
}