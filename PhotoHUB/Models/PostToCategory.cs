namespace PhotoHUB.models;

public class PostToCategory : Entity
{

    public Guid PostId { get; set; }
    public Post Post { get; set; } = null!;

    public Guid CategoryId { get; set; }
    public Category Category { get; set; } = null!;
}