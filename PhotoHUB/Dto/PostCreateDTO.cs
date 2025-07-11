namespace PhotoHUB.DTO;

public class PostCreateDto
{
    public string Name { get; set; } = null!;
    public string Description { get; set; } = null!;
    public string S3Key { get; set; } = null!;
}