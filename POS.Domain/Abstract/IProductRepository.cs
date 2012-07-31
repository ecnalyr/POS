using System;
using POS.Domain.Model;

namespace POS.Domain.Abstract
{
    #region

    using System.Linq;


    #endregion

    public interface IProductRepository : IDisposable
    {
        #region Public Properties

        IQueryable<Category> Categories { get; }

        IQueryable<ParentCategory> ParentCategories { get; }

        IQueryable<Product> Products { get; }

        #endregion

        #region Public Methods and Operators

        void DeleteCategory(Category category);

        void DeleteParentCategory(ParentCategory parentCategory);

        void DeleteProduct(Product product);

        void SaveCategory(Category category);

        void SaveParentCategory(ParentCategory parentCategory);

        void SaveProduct(Product product);

        #endregion
    }
}