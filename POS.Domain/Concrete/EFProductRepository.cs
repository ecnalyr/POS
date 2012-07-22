namespace POS.Domain.Concrete
{
    #region

    using System.Data;
    using System.Linq;

    using POS.Domain.Abstract;
    using POS.Domain.Entities;

    #endregion

    public class EFProductRepository : IProductRepository
    {
        #region Fields

        private readonly EFDbContext context = new EFDbContext();

        #endregion

        #region Public Properties

        public IQueryable<Category> Categories
        {
            get
            {
                return context.Categories;
            }
        }

        public IQueryable<ParentCategory> ParentCategories
        {
            get
            {
                return context.ParentCategories;
            }
        }

        public IQueryable<Product> Products
        {
            get
            {
                return context.Products;
            }
        }

        #endregion

        #region Public Methods and Operators

        public void DeleteProduct(Product product)
        {
            context.Products.Remove(product);
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

        #endregion
    }
}