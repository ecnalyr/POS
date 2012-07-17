using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using POS.Domain.Entities;

namespace POS.Domain.Concrete
{
    public class EFDbContext : DbContext
    {
        public DbSet<Product> Products { get; set; }
        public DbSet<Category> Categories { get; set; } 
    }
}
