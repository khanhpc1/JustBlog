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
    public class PostRepository : GenericRepository<Post>, IPostRepository
    {
        public PostRepository(JustBlogDbContext context) : base(context)
        {
        }

        public int CountPostsForCategory(string category)
        {
            throw new NotImplementedException();
        }

        public int CountPostsForTag(string tag)
        {
            throw new NotImplementedException();
        }

        public async Task<Post> FindPost(int year, int month, string urlSlug)
        {
            var posts = await GetAllAsync();
            return (Post)posts.Where(q => q.PostedOn.Year == year && q.PostedOn.Month == month && q.UrlSlug == urlSlug);           
        }
        

        public async Task<ICollection<Post>> GetLatestPost(int size)
        {
            var posts = await GetAllAsync();
            return posts.OrderByDescending(p => p.PostedOn).Take(size).ToList();
        }

        public async Task<ICollection<Post>> GetPostsByCategory(int categoryId)
        {
            var posts = await GetAllAsync();
            return posts.Where(q => q.CategoryId == categoryId)
                       .ToList();
        }

        public Task<ICollection<Post>> GetPostsByCategory(string category)
        {
            throw new NotImplementedException();
        }

        public async Task<ICollection<Post>> GetPostsByMonth(DateTime monthYear)
        {
            var posts = await GetAllAsync();
            return posts.Where(q => q.PostedOn.Month == monthYear.Month)
                       .ToList();
        }

        public async Task<ICollection<Post>> GetPostsByTag(Tag tag)
        {
            var posts = await GetAllAsync();
            return posts.Where(q => q.PostTagMaps == tag.PostTagMaps)
                       .ToList();
        }

        public Task<ICollection<Post>> GetPostsByTag(string tag)
        {
            throw new NotImplementedException();
        }

        public async Task<ICollection<Post>> GetPublisedPosts()
        {
            var posts = await GetAllAsync();
            return posts.Where(q => q.Published == true)
                       .ToList();
        }

        public async Task<ICollection<Post>> GetUnpublisedPosts()
        {
            var posts = await GetAllAsync();
            return posts.Where(q => q.Published == false)
                       .ToList();
        }

    }
}
