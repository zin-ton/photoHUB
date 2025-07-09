using PhotoHUB.models;

namespace PhotoHUB.configs;
using System;
using Microsoft.EntityFrameworkCore;

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
            entity.Property(e => e.Id).HasDefaultValueSql("gen_random_uuid()");
            entity.Property(e => e.FirstName).IsRequired();
            entity.Property(e => e.LastName).IsRequired();
            entity.Property(e => e.Password).IsRequired();
            entity.Property(e => e.Login).IsRequired();
            entity.Property(e => e.Email).IsRequired();
            entity.Property(e => e.S3Key);
        });
        
        modelBuilder.Entity<Category>(entity =>
        {
            entity.ToTable("category");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).HasDefaultValueSql("gen_random_uuid()");
            entity.Property(e => e.Name).IsRequired();
        });

        modelBuilder.Entity<PostToCategory>(entity =>
        {
            entity.ToTable("post_to_category");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).HasDefaultValueSql("gen_random_uuid()");
            entity.HasIndex(e => e.PostId).IsUnique();
            entity.HasIndex(e => e.CategoryId).IsUnique();

            entity.HasOne(ptc => ptc.Category)
                .WithMany()
                .HasForeignKey(ptc => ptc.CategoryId);
            
            entity.HasOne<Post>()
                .WithOne(p => p.PostToCategory)
                .HasForeignKey<PostToCategory>(ptc => ptc.PostId);
        });
        
        modelBuilder.Entity<Post>(entity =>
        {
            entity.ToTable("post");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).HasDefaultValueSql("gen_random_uuid()");

            entity.HasOne(p => p.User)
                .WithMany(u => u.Posts)
                .HasForeignKey(p => p.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.Property(p => p.DateTime).IsRequired();
            entity.Property(p => p.S3Key).IsRequired();

        });
        
        modelBuilder.Entity<Like>(entity =>
        {
            entity.ToTable("Like");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).HasDefaultValueSql("gen_random_uuid()");

            entity.HasOne(l => l.Post)
                .WithMany(p => p.Likes)
                .HasForeignKey(l => l.PostId);

            entity.HasOne(l => l.User)
                .WithMany(u => u.Likes)
                .HasForeignKey(l => l.UserId);

            entity.Property(l => l.DateTime).IsRequired();
        });
        
        modelBuilder.Entity<SavedPost>(entity =>
        {
            entity.ToTable("saved_posts");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).HasDefaultValueSql("gen_random_uuid()");

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
            entity.Property(e => e.Id).HasDefaultValueSql("gen_random_uuid()");

            entity.HasOne(c => c.Post)
                .WithMany(p => p.Comments)
                .HasForeignKey(c => c.PostId);

            entity.HasOne(c => c.User)
                .WithMany(u => u.Comments)
                .HasForeignKey(c => c.UserId);

            entity.Property(c => c.Content).IsRequired();
            entity.Property(c => c.DateTime).IsRequired();
            
            entity.HasOne(c => c.ReplyTo)
                .WithMany()
                .HasForeignKey(c => c.ReplyToId)
                .OnDelete(DeleteBehavior.Restrict);
        });
        
        modelBuilder.Entity<Collection>(entity =>
        {
            entity.ToTable("collection");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).HasDefaultValueSql("gen_random_uuid()");

            entity.HasOne(c => c.User)
                .WithMany(u => u.Collections)
                .HasForeignKey(c => c.UserId);

            entity.Property(c => c.Name).IsRequired();
        });
        
        modelBuilder.Entity<CollectionPost>(entity =>
        {
            entity.ToTable("collection_posts");
            entity.HasKey(e => new { e.CollectionId, e.PostId });

            entity.HasOne(cp => cp.Collection)
                .WithMany(c => c.CollectionPosts)
                .HasForeignKey(cp => cp.CollectionId);

            entity.HasOne(cp => cp.Post)
                .WithMany(p => p.CollectionPosts)
                .HasForeignKey(cp => cp.PostId);
        });
    }
}