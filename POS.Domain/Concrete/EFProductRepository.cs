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

        #endregion
    }
}