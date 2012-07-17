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

        public IQueryable<Category> Categories
        {
            get { return context.Categories; }
        }
    }
}
