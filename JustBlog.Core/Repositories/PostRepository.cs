using JustBlog.Core.Contracts;
using JustBlog.Models.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
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
            var post = await _db.Posts.FindAsync(id);
            return post;
        }

        public Task<Post> FindPost(int year, int month, string urlSlug)
        {
            throw new NotImplementedException();
        }

        public async Task<ICollection<Post>> GetAll()
        {
            var categories = await _db.Posts.ToListAsync();
            return categories;
        }

        public async Task<Post> GetById(int id)
        {
            var post = await _db.Posts.FindAsync();
            return post;
        }

        public Task<ICollection<Post>> GetLatestPost(int size)
        {
            throw new NotImplementedException();
        }

        public Task<ICollection<Post>> GetPostsByCategory(string category)
        {
            throw new NotImplementedException();
        }

        public Task<ICollection<Post>> GetPostsByMonth(DateTime monthYear)
        {
            throw new NotImplementedException();
        }

        public Task<ICollection<Post>> GetPostsByTag(string tag)
        {
            throw new NotImplementedException();
        }

        public Task<ICollection<Post>> GetPublisedPosts()
        {
            throw new NotImplementedException();
        }

        public Task<ICollection<Post>> GetUnpublisedPosts()
        {
            throw new NotImplementedException();
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
