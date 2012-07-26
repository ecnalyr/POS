﻿namespace POS.Domain.Concrete
{
    using System.Collections.Generic;
    using System.Data.Entity;

    using POS.Domain.Entities;

    public class EfDbContextInitializer : DropCreateDatabaseIfModelChanges<EfDbContext>
    {
        protected override void Seed(EfDbContext context)
        {
            var parentCategories = new List<ParentCategory>
                                       {
                                           new ParentCategory {Name = "SportsEquipment"},
                                           new ParentCategory {Name = "Consumables"}
                                       };
            parentCategories.ForEach(s => context.ParentCategories.Add(s));
            context.SaveChanges();

            var categories = new List<Category>
                                 {
                                     new Category {Name = "Balls", ParentCategoryId = 1},
                                     new Category {Name = "Drinks", ParentCategoryId = 2},
                                     new Category {Name = "Food", ParentCategoryId = 2}
                                 };
            categories.ForEach(s => context.Categories.Add(s));
            context.SaveChanges();

            var products = new List<Product>
                               {
                                   new Product {Name = "Football", Description = "Brown in color", Price = 25, CategoryId = 1 },
                                   new Product {Name = "Soccerball", Description = "White in color", Price = 15, CategoryId = 1},
                                   new Product {Name = "Volleyball", Description = "Colorful in color", Price = 25, CategoryId = 1},
                                   new Product {Name = "Milk", Description = "Brown in color", Price = new decimal(0.75), CategoryId = 2},
                                   new Product {Name = "Water", Description = "Brown in color", Price = new decimal(1.50), CategoryId = 2},
                                   new Product {Name = "Taco", Description = "Folded in half", Price = new decimal(0.75), CategoryId = 3}
                               };
            products.ForEach(s => context.Products.Add(s));
            context.SaveChanges();
        }
    }
}
