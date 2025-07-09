namespace PhotoHUB.models;

public class Collection
{
    public Guid Id { get; set; }

    public Guid UserId { get; set; }
    public User User { get; set; } = null!;

    public string Name { get; set; } = null!;

    public ICollection<CollectionPost> CollectionPosts { get; set; } = new List<CollectionPost>();
}