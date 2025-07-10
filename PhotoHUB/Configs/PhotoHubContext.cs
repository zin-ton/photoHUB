using PhotoHUB.models;
using Microsoft.EntityFrameworkCore;

namespace PhotoHUB.configs;

public class PhotoHubContext : DbContext
{
   public PhotoHubContext(DbContextOptions<PhotoHubContext> options)
        : base(options)
    {
    }

    public DbSet<User> Users { get; set; }
    public DbSet<Category> Categories { get; set; }
    public DbSet<Post> Posts { get; set; }
    public DbSet<PostToCategory> PostToCategories { get; set; }
    public DbSet<Like> Likes { get; set; }
    public DbSet<SavedPost> SavedPosts { get; set; }
    public DbSet<Comment> Comments { get; set; }
    public DbSet<Collection> Collections { get; set; }
    public DbSet<CollectionPost> CollectionPosts { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<User>(entity =>
        {
            entity.ToTable("user");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).HasDefaultValueSql("gen_random_uuid()").HasColumnName("id");
            entity.Property(e => e.FirstName).IsRequired().HasColumnName("first_name");
            entity.Property(e => e.LastName).IsRequired().HasColumnName("last_name");
            entity.Property(e => e.Password).IsRequired().HasColumnName("password");
            entity.Property(e => e.Login).IsRequired().HasColumnName("login");
            entity.Property(e => e.Email).IsRequired().HasColumnName("email");
            entity.Property(e => e.S3Key).HasColumnName("s3_key");
        });
        
        modelBuilder.Entity<Category>(entity =>
        {
            entity.ToTable("category");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).HasDefaultValueSql("gen_random_uuid()").HasColumnName("id");
            entity.Property(e => e.Name).IsRequired().HasColumnName("name");
        });

        modelBuilder.Entity<PostToCategory>(entity =>
        {
            entity.ToTable("post_to_category");

            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id)
                .HasDefaultValueSql("gen_random_uuid()")
                .HasColumnName("id");
            entity.Property(e => e.PostId).HasColumnName("post_id");
            entity.Property(e => e.CategoryId).HasColumnName("category_id");
            entity.HasIndex(e => e.PostId);
            entity.HasIndex(e => e.CategoryId);

            entity.HasOne(ptc => ptc.Post)
                .WithMany(p => p.PostToCategories)
                .HasForeignKey(ptc => ptc.PostId);

            entity.HasOne(ptc => ptc.Category)
                .WithMany(c => c.PostToCategories)
                .HasForeignKey(ptc => ptc.CategoryId);
        });

        
        modelBuilder.Entity<Post>(entity =>
        {
            entity.ToTable("post");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).HasDefaultValueSql("gen_random_uuid()").HasColumnName("id");
            entity.Property(e => e.Name).IsRequired().HasColumnName("name");
            entity.Property(e => e.Description).HasColumnName("description");
            entity.HasOne(p => p.User)
                .WithMany(u => u.Posts)
                .HasForeignKey(p => p.UserId)
                .OnDelete(DeleteBehavior.Cascade);
            entity.Property(p => p.UserId).HasColumnName("user_id");
            entity.Property(p => p.DateTime).IsRequired().HasColumnName("datetime");
            entity.Property(p => p.S3Key).IsRequired().HasColumnName("s3_key");

        });
        
        modelBuilder.Entity<Like>(entity =>
        {
            entity.ToTable("Like");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).HasDefaultValueSql("gen_random_uuid()").HasColumnName("id");

            entity.HasOne(l => l.Post)
                .WithMany(p => p.Likes)
                .HasForeignKey(l => l.PostId);

            entity.HasOne(l => l.User)
                .WithMany(u => u.Likes)
                .HasForeignKey(l => l.UserId);
            entity.Property(l => l.UserId).HasColumnName("user_id");
            entity.Property(l => l.PostId).HasColumnName("post_id");
            entity.Property(l => l.DateTime).IsRequired().HasColumnName("date_time");
        });
        
        modelBuilder.Entity<SavedPost>(entity =>
        {
            entity.ToTable("saved_posts");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).HasDefaultValueSql("gen_random_uuid()").HasColumnName("id");
            entity.Property(e => e.PostId).HasColumnName("post_id");
            entity.Property(e => e.UserId).HasColumnName("user_id");
            entity.HasOne(sp => sp.Post)
                .WithMany(p => p.SavedPosts)
                .HasForeignKey(sp => sp.PostId);

            entity.HasOne(sp => sp.User)
                .WithMany(u => u.SavedPosts)
                .HasForeignKey(sp => sp.UserId);
        });
        
        modelBuilder.Entity<Comment>(entity =>
        {
            entity.ToTable("comment");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).HasDefaultValueSql("gen_random_uuid()").HasColumnName("id");
            entity.Property(e => e.PostId).HasColumnName("post_id");
            
            entity.HasOne(c => c.Post)
                .WithMany(p => p.Comments)
                .HasForeignKey(c => c.PostId);

            entity.HasOne(c => c.User)
                .WithMany(u => u.Comments)
                .HasForeignKey(c => c.UserId);
            entity.Property(c => c.UserId).HasColumnName("user_id");
            entity.Property(c => c.ReplyToId).HasColumnName("reply_to");
            entity.Property(c => c.Content).IsRequired().HasColumnName("content");
            entity.Property(c => c.DateTime).IsRequired().HasColumnName("date_time");
            
            entity.HasOne(c => c.ReplyTo)
                .WithMany()
                .HasForeignKey(c => c.ReplyToId)
                .OnDelete(DeleteBehavior.Restrict);
        });
        
        modelBuilder.Entity<Collection>(entity =>
        {
            entity.ToTable("collection");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).HasDefaultValueSql("gen_random_uuid()").HasColumnName("id");

            entity.HasOne(c => c.User)
                .WithMany(u => u.Collections)
                .HasForeignKey(c => c.UserId);
            entity.Property(c => c.UserId).HasColumnName("user_id");
            entity.Property(c => c.Name).IsRequired().HasColumnName("name");
        });
        
        modelBuilder.Entity<CollectionPost>(entity =>
        {
            entity.ToTable("collection_posts");
            entity.Property(e => e.Id).HasDefaultValueSql("gen_random_uuid()").HasColumnName("id");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.PostId).HasColumnName("post_id");
            entity.Property(e => e.CollectionId).HasColumnName("collection_id");
            entity.HasOne(cp => cp.Collection)
                .WithMany(c => c.CollectionPosts)
                .HasForeignKey(cp => cp.CollectionId);

            entity.HasOne(cp => cp.Post)
                .WithMany(p => p.CollectionPosts)
                .HasForeignKey(cp => cp.PostId);
        });
    }
}