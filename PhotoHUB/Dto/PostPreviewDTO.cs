namespace PhotoHUB.DTO;

public class PostPreviewDto
{
    public Guid Guid { get; set; }
    public string Name { get; set; } = null!;
    public string Description { get; set; } = null!;
    public string S3Key { get; set; } = null!;
    public DateTime DateTime { get; set; }
    public string AuthorLogin { get; set; } = null!;
}