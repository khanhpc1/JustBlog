using Microsoft.EntityFrameworkCore.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace JustBlog.Core.Contracts
{
    public interface IGenericRepository<TEntity> where TEntity : class
    {
        Task<ICollection<TEntity>> GetAllAsync(
           Expression<Func<TEntity, bool>> expression = null,
           Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
           Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include = null);

        Task<TEntity> GetFirstOrDefaultAsync(Expression<Func<TEntity, bool>> expression =null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include = null);

        Task<bool> IsExistsAsync(Expression<Func<TEntity, bool>> expression);

        Task CreateAsync(TEntity entity);

        void Update(TEntity entity);

        void Delete(TEntity entity);

        void Delete(object id);

    }
}
