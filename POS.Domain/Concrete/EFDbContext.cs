namespace POS.Domain.Concrete
{
    #region

    using System.Data.Entity;

    using POS.Domain.Entities;

    #endregion

    public class EFDbContext : DbContext
    {
        #region Public Properties

        public DbSet<Category> Categories { get; set; }

        public DbSet<ParentCategory> ParentCategories { get; set; }

        public DbSet<Product> Products { get; set; }

        #endregion
    }
}