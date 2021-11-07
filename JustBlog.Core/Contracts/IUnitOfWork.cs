using JustBlog.Models.Entities;
using System;
using System.Threading.Tasks;

namespace JustBlog.Core.Contracts
{
    public interface IUnitOfWork : IDisposable
    {
        IGenericRepository<Category> Categories { get; }
        IGenericRepository<Post> Posts { get; }
        IGenericRepository<Tag> Tags { get; }
        Task Save();

    }
}