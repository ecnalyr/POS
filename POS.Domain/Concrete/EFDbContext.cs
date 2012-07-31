﻿namespace POS.Domain.Concrete
{
    #region

    using System.Data.Entity;

    using POS.Domain.Model;

    #endregion

    public class EfDbContext : DbContext
    {
        #region Public Properties

        public DbSet<Category> Categories { get; set; }

        public DbSet<ParentCategory> ParentCategories { get; set; }

        public DbSet<Product> Products { get; set; }

        public DbSet<Establishment> Establishments { get; set; }

        #endregion
    }
}