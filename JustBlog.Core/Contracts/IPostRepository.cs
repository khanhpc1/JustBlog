using JustBlog.Models.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace JustBlog.Core.Contracts
{
    public interface IPostRepository : IRepositoryBase<Post>
    {
        Task<Post> FindPost(int year, int month, string urlSlug);
        Task<ICollection<Post>> GetPublisedPosts();
        Task<ICollection<Post>> GetUnpublisedPosts();
        Task<ICollection<Post>> GetLatestPost(int size);
        Task<ICollection<Post>> GetPostsByMonth(DateTime monthYear);
        int CountPostsForCategory(string category);
        Task<ICollection<Post>> GetPostsByCategory(string category);
        int CountPostsForTag(string tag);
        Task<ICollection<Post>> GetPostsByTag(string tag);
    }
}