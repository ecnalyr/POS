using System.Linq;
using POS.Domain.Entities;

namespace POS.Domain.Abstract
{
    public interface IProductRepository
    {
        IQueryable<Product> Products { get; }

        void SaveProduct(Product product);

        void DeleteProduct(Product product);

        IQueryable<Category> Categories { get; }
        IQueryable<ParentCategory> ParentCategories { get; }
    }
}
