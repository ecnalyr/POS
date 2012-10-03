using System;
using System.Linq;
using System.Linq.Expressions;

namespace POS.Domain.Abstract
{
    public interface IGenericRepository<T> : IDisposable where T : class
    {
        IQueryable<T> GetAll();
        IQueryable<T> FindBy(Expression<Func<T, bool>> predicate);
        void Add(T entity);
        void Delete(T entity);
        void Edit(T entity);
        void Save();
    }
}
