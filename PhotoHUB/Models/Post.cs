using System.ComponentModel.DataAnnotations.Schema;

namespace PhotoHUB.models;

public class Post : Entity
{
    [Column("user_id")]
    public Guid UserId { get; set; }
    public User User { get; set; } = null!;
    public string Name { get; set; } = null!;
    public string Description { get; set; } = null!;
    public DateTime DateTime { get; set; }
    public string S3Key { get; set; } = null!;
    public ICollection<PostToCategory> PostToCategories { get; set; } = new List<PostToCategory>();    
    public ICollection<Like> Likes { get; set; } = new List<Like>();
    public ICollection<SavedPost> SavedPosts { get; set; } = new List<SavedPost>();
    public ICollection<Comment> Comments { get; set; } = new List<Comment>();
    public ICollection<CollectionPost> CollectionPosts { get; set; } = new List<CollectionPost>();
}