using System.Linq;
using POS.Domain.Entities;

namespace POS.Domain.Abstract
{
    public interface IProductRepository
    {
        IQueryable<Product> Products { get; }
        IQueryable<Category> Categories { get; }
    }
}
