using System;
using System.Data;
using System.Linq;
using POS.Domain.Abstract;
using POS.Domain.Model;

namespace POS.Infrastructure.Concrete
{
    #region

    

    #endregion

    public class EfProductRepository : IProductRepository
    {
        #region Fields

        private readonly EfDbContext context = new EfDbContext();

        #endregion

        #region Public Properties

        public IQueryable<Category> Categories
        {
            get { return context.Categories; }
        }

        public IQueryable<ParentCategory> ParentCategories
        {
            get { return context.ParentCategories; }
        }

        public IQueryable<Product> Products
        {
            get { return context.Products; }
        }

        #endregion

        #region Public Methods and Operators

        private bool _disposed;

        public void DeleteCategory(Category category)
        {
            context.Categories.Remove(category);
            context.SaveChanges();
        }

        public void DeleteParentCategory(ParentCategory parentCategory)
        {
            context.ParentCategories.Remove(parentCategory);
            context.SaveChanges();
        }

        public void DeleteProduct(Product product)
        {
            context.Products.Remove(product);
            context.SaveChanges();
        }

        public void SaveCategory(Category category)
        {
            if (category.CategoryId == 0)
            {
                context.Categories.Add(category);
            }
            else
            {
                context.Entry(category).State = EntityState.Modified;
            }

            context.SaveChanges();
        }

        public void SaveParentCategory(ParentCategory parentCategory)
        {
            if (parentCategory.ParentCategoryId == 0)
            {
                context.ParentCategories.Add(parentCategory);
            }
            else
            {
                context.Entry(parentCategory).State = EntityState.Modified;
            }

            context.SaveChanges();
        }

        public void SaveProduct(Product product)
        {
            if (product.ProductId == 0)
            {
                context.Products.Add(product);
            }
            else
            {
                context.Entry(product).State = EntityState.Modified;
            }

            context.SaveChanges();
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    context.Dispose();
                }
            }
            _disposed = true;
        }

        #endregion
    }
}