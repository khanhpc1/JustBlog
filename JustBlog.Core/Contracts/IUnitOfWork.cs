using JustBlog.Models.Entities;
using System;
using System.Threading.Tasks;

namespace JustBlog.Core.Contracts
{
    public interface IUnitOfWork : IDisposable
    {
        IGenericRepository<TEntity> GetRepository<TEntity>(bool hasCustomRepository = false) where TEntity : class;
        ICategoryRepository CategoryRepository { get; }

        IPostRepository PostRepository { get; }

        ITagRepository TagRepository { get; }

        JustBlogDbContext Context { get; }


        int SaveChanges(bool ensureAutoHistory = false);
        Task<int> SaveChangesAsync(bool ensureAutoHistory = false);
    }
}