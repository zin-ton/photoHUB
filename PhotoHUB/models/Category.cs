namespace PhotoHUB.models;

public class Category
{
    public Guid Id { get; set; }
    public string Name { get; set; } = null!;
    
    public ICollection<PostToCategory> PostToCategories { get; set; } = new List<PostToCategory>();
}