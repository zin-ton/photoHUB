namespace PhotoHUB.models;

public class Post
{
    public Guid Id { get; set; }

    public Guid UserId { get; set; }
    public User User { get; set; } = null!;

    public DateTime DateTime { get; set; }
    public string S3Key { get; set; } = null!;
    
    public PostToCategory? PostToCategory { get; set; }
    public ICollection<Like> Likes { get; set; } = new List<Like>();
    public ICollection<SavedPost> SavedPosts { get; set; } = new List<SavedPost>();
    public ICollection<Comment> Comments { get; set; } = new List<Comment>();
    public ICollection<CollectionPost> CollectionPosts { get; set; } = new List<CollectionPost>();
}