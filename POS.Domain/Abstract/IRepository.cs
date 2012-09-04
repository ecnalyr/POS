using System.Linq;

namespace POS.Domain.Abstract
{
    public interface IRepository<TEntity> where TEntity : class
    {
        IQueryable<TEntity> Query();
        void Delete(TEntity entity);
        void Add(TEntity entity);
        void Update(TEntity entity);
    }
}
