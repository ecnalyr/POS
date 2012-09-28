using System;
using System.Collections.Generic;
using System.Data.Entity;
using POS.Domain.Model;
using System.Web.Security;
using POS.Infrastructure.Membership;

namespace POS.Infrastructure
{
    public class EfDbContextInitializer : DropCreateDatabaseIfModelChanges<EfDbContext>
    {
        protected override void Seed(EfDbContext context)
        {
            WebSecurity.Register("Admin", "pas5word", "demo@demo.com", true, "Demo", "Demo");
            Roles.CreateRole("Admin");
            Roles.AddUserToRole("Demo", "Admin");

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
                                   new Product {Name = "Football", Description = "Brown in color", Price = 25, CategoryId = 1, EstablishmentId = 1},
                                   new Product {Name = "Soccerball", Description = "White in color", Price = 15, CategoryId = 1, EstablishmentId = 1},
                                   new Product {Name = "Volleyball", Description = "Colorful in color", Price = 25, CategoryId = 1, EstablishmentId = 1},
                                   new Product {Name = "Milk", Description = "Brown in color", Price = new decimal(0.75), CategoryId = 2, EstablishmentId = 1},
                                   new Product {Name = "Water", Description = "Brown in color", Price = new decimal(1.50), CategoryId = 2, EstablishmentId = 1},
                                   new Product {Name = "Taco", Description = "Folded in half", Price = new decimal(0.75), CategoryId = 3, EstablishmentId = 1}
                               };
            products.ForEach(s => context.Products.Add(s));

            var estabslishments = new List<Establishment>
                {
                    new Establishment {Name = "The Dreary Inn"},
                    new Establishment {Name = "Falling Out the Third"},
                    new Establishment {Name = "The Rim of the Sky"}
                };
            estabslishments.ForEach(s => context.Establishments.Add(s));

            var promos = new List<Promo>
                {
                    new Promo {Description = "Fifty Percent Off", PercentOff = (float) .50},
                    new Promo {Description = "Seventy-Five Percent Off", PercentOff = (float) .75}
                };
            promos.ForEach(s => context.Promos.Add(s));

            context.SaveChanges();

            var lineItemPromos = new List<LineItemPromo>
                {
                    new LineItemPromo {PromoId = 1},
                    new LineItemPromo {PromoId = 2}
                };
            lineItemPromos.ForEach(s => context.LineItemPromos.Add(s));

            context.SaveChanges();

            var orderDetails = new List<OrderDetail>
                {
                    /*new OrderDetail { OrderId = 1, Quantity = 2, ProductName = "Basketball", UnitPrice = 14, LineItemPromoId = 2},
                    new OrderDetail { OrderId = 1, Quantity = 1, ProductName = "Tennis Racket", UnitPrice = 47, LineItemPromoId = 1},
                    new OrderDetail { OrderId = 1, Quantity = 3, ProductName = "Tennis Ball", UnitPrice = 6},
                    new OrderDetail { OrderId = 2, Quantity = 2, ProductName = "Basketball", UnitPrice = 14},
                    new OrderDetail { OrderId = 2, Quantity = 1, ProductName = "Tennis Racket", UnitPrice = 47},
                    new OrderDetail { OrderId = 2, Quantity = 3, ProductName = "Tennis Ball", UnitPrice = 6},
                    new OrderDetail { OrderId = 3, Quantity = 1, ProductName = "Basketball", UnitPrice = 14}*/

                };
            orderDetails.ForEach(s => context.OrderDetails.Add(s));

            var orders = new List<Order>
                {       
                    new Order { OrderDetails = new List<OrderDetail>()
                        {
                            new OrderDetail { OrderId = 1, Quantity = 2, ProductName = "Basketball", UnitPrice = 14, LineItemPromoId = 2},
                            new OrderDetail { OrderId = 1, Quantity = 1, ProductName = "Tennis Racket", UnitPrice = 47, LineItemPromoId = 1},
                            new OrderDetail { OrderId = 1, Quantity = 3, ProductName = "Tennis Ball", UnitPrice = 6}
                        },
                    EstablishmentId = 1, TotalCost = (decimal) 62.5, SalesTax = (decimal) 5.15625, CustomerName = "Albert", TimeProcessed = DateTime.Now.AddHours(-3.00)},

                    new Order { OrderDetails = new List<OrderDetail>()
                        {
                            new OrderDetail { OrderId = 2, Quantity = 2, ProductName = "Basketball", UnitPrice = 14},
                            new OrderDetail { OrderId = 2, Quantity = 1, ProductName = "Tennis Racket", UnitPrice = 47},
                            new OrderDetail { OrderId = 2, Quantity = 3, ProductName = "Tennis Ball", UnitPrice = 6},
                        },
                    EstablishmentId = 1, TotalCost = 93, SalesTax = (decimal) 7.6725, CustomerName = "Henry", TimeProcessed = DateTime.Now},

                    new Order { OrderDetails = new List<OrderDetail>()
                        {
                            new OrderDetail { OrderId = 3, Quantity = 1, ProductName = "Basketball", UnitPrice = 14}
                        },
                    EstablishmentId = 1, TotalCost = 14, SalesTax = (decimal) 1.155, CustomerName = "Thomas", TimeProcessed = DateTime.Now},
                };
            orders.ForEach(s => context.Orders.Add(s));

            context.SaveChanges();
        }

    }
}
