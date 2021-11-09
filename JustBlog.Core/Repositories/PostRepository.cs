using JustBlog.Core.Contracts;
using JustBlog.Models.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace JustBlog.Core.Repositories
{
    public class PostRepository : IPostRepository
    {
        private readonly JustBlogDbContext _db;
        public PostRepository(JustBlogDbContext db)
        {
            _db = db;  
        }

        public int CountPostsForCategory(string category)
        {
            throw new NotImplementedException();
        }

        public int CountPostsForTag(string tag)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> Create(Post entity)
        {
            await _db.Posts.AddAsync(entity);
            return await Save();
        }

        public async Task<bool> Delete(Post entity)
        {
            _db.Posts.Remove(entity);
            return await Save();
        }

        public Task<bool> Delete(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<Post> Find(int id)
        {
            var post = await _db.Posts
                .Include(q => q.Category)
                .Include(q => q.PostTagMaps)
                .FirstOrDefaultAsync(p => p.Id == id);
            return post;
        }

        public async Task<Post> FindPost(int year, int month, string urlSlug)
        {
            var posts = await GetAll();
            return (Post)posts.Where(q => q.PostedOn.Year == year && q.PostedOn.Month == month && q.UrlSlug == urlSlug);           
        }

        public async Task<ICollection<Post>> GetAll()
        {
            var posts = await _db.Posts
                .Include(q => q.Category)
                .Include(q => q.PostTagMaps)
                .ToListAsync();
            return posts;
        }

        public async Task<ICollection<Post>> GetAll(Expression<Func<Post, bool>> expression = null, Func<IQueryable<Post>, IOrderedQueryable<Post>> orderBy = null, Func<IQueryable<Post>, IIncludableQueryable<Post, object>> includes = null)
        {
            var posts = await _db.Posts
                 .Include(q => q.Category)
                 .Include(q => q.PostTagMaps)
                 .ToListAsync();
            return posts;
        }

        public async Task<Post> GetById(int id)
        {
            var post = await _db.Posts.FindAsync();
            return post;
        }

        public async Task<ICollection<Post>> GetLatestPost(int size)
        {
            var posts = await GetAll();
            return posts.OrderByDescending(p => p.PostedOn).Take(size).ToList();
        }

        public async Task<ICollection<Post>> GetPostsByCategory(int categoryId)
        {
            var posts = await GetAll();
            return posts.Where(q => q.CategoryId == categoryId)
                       .ToList();
        }

        public Task<ICollection<Post>> GetPostsByCategory(string category)
        {
            throw new NotImplementedException();
        }

        public async Task<ICollection<Post>> GetPostsByMonth(DateTime monthYear)
        {
            var posts = await GetAll();
            return posts.Where(q => q.PostedOn.Month == monthYear.Month)
                       .ToList();
        }

        public async Task<ICollection<Post>> GetPostsByTag(Tag tag)
        {
            var posts = await GetAll();
            return posts.Where(q => q.PostTagMaps == tag.PostTagMaps)
                       .ToList();
        }

        public Task<ICollection<Post>> GetPostsByTag(string tag)
        {
            throw new NotImplementedException();
        }

        public async Task<ICollection<Post>> GetPublisedPosts()
        {
            var posts = await GetAll();
            return posts.Where(q => q.Published == true)
                       .ToList();
        }

        public async Task<ICollection<Post>> GetUnpublisedPosts()
        {
            var posts = await GetAll();
            return posts.Where(q => q.Published == false)
                       .ToList();
        }

        public async Task<bool> IsExists(int id)
        {
            var exists = await _db.Posts.AnyAsync(c => c.Id == id);
            return exists;
        }

        public async Task<bool> Save()
        {
            var changes = await _db.SaveChangesAsync();
            return changes > 0;
        }

        public async Task<bool> Update(Post entity)
        {
            _db.Posts.Update(entity);

            return await Save();
        }
    }
}
