using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using POS.Domain.Abstract;
using POS.Domain.Entities;

namespace POS.Domain.Concrete
{
    public class EFProductRepository : IProductRepository
    {
        private EFDbContext context = new EFDbContext();

        public IQueryable<Product> Products
        {
            get { return context.Products; }
        }

        public void SaveProduct(Product product)
        {
            if (product.ProductId == 0)
            {
                context.Products.Add(product);
            }
            else
            {
                context.Entry(product).State = System.Data.EntityState.Modified;
            }
            context.SaveChanges();
        }

        public void DeleteProduct(Product product)
        {
            context.Products.Remove(product);
            context.SaveChanges();
        }

        public IQueryable<Category> Categories
        {
            get { return context.Categories; }
        }

        public IQueryable<ParentCategory> ParentCategories
        {
            get { return context.ParentCategories; }
        }
    }
}
