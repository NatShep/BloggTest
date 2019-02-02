using Microsoft.EntityFrameworkCore;

namespace BlogAppVer5.BdModels
{
    public class BlogContext: DbContext
    {
        public DbSet<Post> Posts { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<PostTag> PostTags { get; set; }

        public BlogContext(DbContextOptions<BlogContext> options) 
            :base(options)
        {
            Database.EnsureCreated();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<PostTag>()
                .HasKey(t =>new { t.PostId, t.TagId});

            modelBuilder.Entity<PostTag>()
                .HasOne(pt => pt.Post)
                .WithMany(t => t.PostTags)
                .HasForeignKey(pt => pt.PostId);

            modelBuilder.Entity<PostTag>()
               .HasOne(pt => pt.Tag)
               .WithMany(t => t.PostTags)
               .HasForeignKey(pt => pt.TagId);       
        }
    }
}
