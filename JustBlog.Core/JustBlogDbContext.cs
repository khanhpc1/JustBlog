using JustBlog.Models.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace JustBlog.Core
{
    public class JustBlogDbContext : IdentityDbContext<AppUser>
    {
        
        public JustBlogDbContext(DbContextOptions<JustBlogDbContext> options) : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Server=KHANHPC;Database=JustBlog;Trusted_Connection = True ");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Bỏ tiền tố AspNet của các bảng: mặc định các bảng trong IdentityDbContext có
            // tên với tiền tố AspNet như: AspNetUserRoles, AspNetUser ...
            // Đoạn mã sau chạy khi khởi tạo DbContext, tạo database sẽ loại bỏ tiền tố đó
            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                var tableName = entityType.GetTableName();
                if (tableName.StartsWith("AspNet"))
                {
                    entityType.SetTableName(tableName.Substring(6));
                }
            }

            //Set up one-many between post table and category table
            modelBuilder.Entity<Post>()
                 .HasOne<Category>(p => p.Category)
                 .WithMany(c => c.Posts)
                 .HasForeignKey(p => p.CategoryId);

            //create index for UrlSlug  in category table
            modelBuilder.Entity<Category>(entity =>
            {
                entity.HasIndex(c => c.UrlSlug);
            });

            modelBuilder.Entity<PostTagMap>().HasKey(pt => new { pt.PostId, pt.TagId });

            modelBuilder.Entity<Post>(post =>
                   post.HasKey(p => p.Id));

            modelBuilder.Entity<Tag>(tag =>
                    tag.HasKey(t => t.Id));

            modelBuilder.Entity<Category>(category =>
                    category.HasKey(c => c.Id));

            //Set up many-many between post table and tag table
            modelBuilder.Entity<PostTagMap>()
                .HasOne<Post>(pt => pt.Post)
                .WithMany(p => p.PostTagMaps)
                .HasForeignKey(pt => pt.PostId);

            modelBuilder.Entity<PostTagMap>()
                .HasOne<Tag>(pt => pt.Tag)
                .WithMany(t => t.PostTagMaps)
                .HasForeignKey(pt => pt.TagId);
        }

        public DbSet<Category> Categories { get; set; }
        public DbSet<Post> Posts { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<PostTagMap> PostTagMaps { get; set; }
    }
}