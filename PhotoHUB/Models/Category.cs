namespace PhotoHUB.models;

public class Category : Entity
{
    public string Name { get; set; } = null!;
    
    public ICollection<PostToCategory> PostToCategories { get; set; } = new List<PostToCategory>();
}