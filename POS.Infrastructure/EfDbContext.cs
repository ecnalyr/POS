using System.Data.Entity;
using POS.Domain.Model;


namespace POS.Infrastructure
{
    #region

    

    #endregion

    public class EfDbContext : DbContext
    {
        #region Public Properties

        public DbSet<Category> Categories { get; set; }

        public DbSet<ParentCategory> ParentCategories { get; set; }

        public DbSet<Product> Products { get; set; }

        public DbSet<Establishment> Establishments { get; set; }

        public DbSet<Order> Orders { get; set; }

        public DbSet<OrderDetail> OrderDetails { get; set; }

        public DbSet<Promo> Promos { get; set; }

        public DbSet<LineItemPromo> LineItemPromos { get; set; }

        public DbSet<User> Users { get; set; }

        public DbSet<Role> Roles { get; set; }
        
        #endregion
    }
}