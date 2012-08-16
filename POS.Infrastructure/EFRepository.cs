using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Text;
using POS.Domain.Abstract;
using System.Data.Objects;

namespace POS.Infrastructure
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private EfDbContext _context;
        public Repository(EfDbContext context)
        {
            _context = context;
        }
        private IDbSet<T> DbSet
        {
            get
            {
                return _context.Set<T>();
            }
        }

        #region IRepository<T> Members

        public IQueryable<T> Query()
        {
            return DbSet.AsQueryable();
        }

        public void Delete(T entity)
        {
            DbSet.Remove(entity);
            _context.SaveChanges();
        }

        public void Add(T entity)
        {
            DbSet.Add(entity);
            _context.SaveChanges();
        }

        public void Update(T entity)
        {
            _context.Entry(entity).State = EntityState.Modified;
            _context.SaveChanges();

        _context.SaveChanges();
        }
        #endregion

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_context != null)
                {
                    _context.Dispose();
                    _context = null;
                }
            }
        }
    } 
}
