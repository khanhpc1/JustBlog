using JustBlog.Core.Contracts;
using JustBlog.Models.Base;
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
    public class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : class
    {
        private readonly JustBlogDbContext _context;
        private readonly DbSet<TEntity> _db;

        public GenericRepository(JustBlogDbContext context)
        {
            _context = context;
            _db = _context.Set<TEntity>();
        }
        public async Task Create(TEntity entity)
        {
            await _db.AddAsync(entity);   
        }

        public void Delete(TEntity entity)
        {
            _db.Remove(entity);
        }

        public async Task<TEntity> Find(Expression<Func<TEntity, bool>> filter, Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> includes = null)
        {
            IQueryable<TEntity> query = _db;
            if(includes != null)
            {
                query = includes(query);
            }
            return await query.FirstOrDefaultAsync(filter);
        }

        public async Task<IEnumerable<TEntity>> GetAll(Expression<Func<TEntity, bool>> filter = null, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null, Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> includes = null)
        {
            IQueryable<TEntity> query = _db;
            
            if(filter!= null)
            {
                query = query.Where(filter);
            }

            if(includes!= null)
            {
                query = includes(query);
            }

            if(orderBy != null)
            {
                query = orderBy(query);
            }
          
            return await query.ToListAsync();
        }

        public Task<TEntity> GetById(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> IsExists(Expression<Func<TEntity, bool>> filter = null)
        {
            IQueryable<TEntity> query = _db;
            return await query.AnyAsync(filter);
        }

        public void Update(TEntity entity)
        {
            _db.Update(entity);
        }
    }
}
