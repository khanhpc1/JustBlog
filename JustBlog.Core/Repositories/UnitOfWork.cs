using JustBlog.Core.Contracts;
using JustBlog.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JustBlog.Core.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly JustBlogDbContext _context;
        private IGenericRepository<Category> _category;
        private IGenericRepository<Post> _post;
        private IGenericRepository<Tag> _tag;

        public UnitOfWork(JustBlogDbContext context)
        {
            _context = context;
        }
        public IGenericRepository<Category> Categories 
            => _category ??= new GenericRepository<Category>(_context);

        public IGenericRepository<Post> Posts 
            => _post ??= new GenericRepository<Post>(_context);

        public IGenericRepository<Tag> Tags
            => _tag ??= new GenericRepository<Tag>(_context);

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool dispose)
        {
            if (dispose)
            {
                _context.Dispose();
            }
        }

        public async Task Save()
        {
            await _context.SaveChangesAsync();
        }

    }
}
