using System.Collections.Generic;
using System.Threading.Tasks;

namespace JustBlog.Core.Contracts
{
    public interface IRepositoryBase<TEntity> where TEntity : class
    {
        Task<ICollection<TEntity>> GetAll();

        Task<TEntity> Find(int id);

        Task<TEntity> GetById(int id);

        Task<bool> IsExists(int id);

        Task<bool> Create(TEntity entity);

        Task<bool> Update(TEntity entity);

        Task<bool> Delete(TEntity entity);

        Task<bool> Delete(int id);

        Task<bool> Save();
    }
}