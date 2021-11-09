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
        Task Create(TEntity entity);
        void Update(TEntity entity);
        void Delete(TEntity entity);

        /// <summary>
        /// Gets all entities. This method is not recommended
        /// </summary>
        /// <param name="filter">A function to test each element for a condition.</param>
        /// <param name="orderBy">A function to order elements.</param>
        /// <param name="include">A function to include navigation properties</param>
        Task<IEnumerable<TEntity>> GetAll(
            Expression<Func<TEntity, bool>> filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> includes = null
            );
        Task<TEntity> GetById(int id);
        Task<TEntity> Find(
            Expression<Func<TEntity, bool>> filter,
            Func<IQueryable<TEntity>,
            IIncludableQueryable<TEntity, object>> includes = null);
        Task<bool> IsExists(Expression<Func<TEntity, bool>> filter);
    }
}
