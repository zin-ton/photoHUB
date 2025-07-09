namespace PhotoHUB.models;

public class User : Entity
{
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public string Password { get; set; } = null!;
    public string Login { get; set; } = null!;
    public string? S3Key { get; set; }
    public string Email { get; set; } = null!;

    public ICollection<Post> Posts { get; set; } = new List<Post>();
    public ICollection<Like> Likes { get; set; } = new List<Like>();
    public ICollection<SavedPost> SavedPosts { get; set; } = new List<SavedPost>();
    public ICollection<Comment> Comments { get; set; } = new List<Comment>();
    public ICollection<Collection> Collections { get; set; } = new List<Collection>();
}