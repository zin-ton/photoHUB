namespace PhotoHUB.models;

public class CollectionPost : Entity
{
    public Guid CollectionId { get; set; }
    public Collection Collection { get; set; } = null!;

    public Guid PostId { get; set; }
    public Post Post { get; set; } = null!;
}