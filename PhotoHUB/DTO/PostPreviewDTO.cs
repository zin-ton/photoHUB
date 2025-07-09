namespace PhotoHUB.DTO;

public class PostPreviewDTO
{
    public Guid Guid { get; set; }
    public string Name { get; set; } = null!;
    public string Description { get; set; } = null!;
    public string S3Key { get; set; } = null!;
    public DateTime DateTime { get; set; }
}