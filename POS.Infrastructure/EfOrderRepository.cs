using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using POS.Domain.Abstract;
using POS.Domain.Model;

namespace POS.Infrastructure
{
    public class EfOrderRepository : IOrderProcessor
    {
        #region Fields

        private readonly EfDbContext context = new EfDbContext();

        #endregion

        #region Public Properties

        public IQueryable<Order> Orders
        {
            get { return context.Orders; }
        }

        #endregion

        private bool _disposed;

        #region IOrderProcessor Members

        public void DeleteOrder(Order order)
        {
            context.Orders.Remove(order);
            context.SaveChanges();
        }

        public void ProcessOrder(Cart cart, ShippingDetails shippingDetails)
        {
            //var order = ProcessTheOrder(cart, shippingDetails);
            //context.Orders.Add(order);
            context.SaveChanges();
            cart.Clear();
        }

        public void SaveOrder(Order order)
        {
            if (order.OrderId == 0)
            {
                context.Orders.Add(order);
            }
            else
            {
                context.Entry(order).State = EntityState.Modified;
            }

            context.SaveChanges();
        }

        /*private Order ProcessTheOrder(Cart cart, ShippingDetails shippingDetails)
        {
            var firstOrDefault = cart.Lines.FirstOrDefault();
            if (firstOrDefault != null) Debug.Write(firstOrDefault.Product.Name);

            var order = new Order();

            var cartLines = new List<CartLine>();
            try
            {
                foreach (var item in cartLines)
                {
                    var orderDetail = new OrderDetail
                        {
                            OrderId = order.OrderId,
                            ProductId = item.Product.ProductId,
                            Quantity = item.Quantity,
                            UnitPrice = item.Product.Price
                        };
                    // I could update the order's total cost here if I wanted
                    context.OrderDetails.Add(orderDetail);
                    order.OrderDetails.Add(orderDetail);
                }
                cartLines.AddRange(cart.Lines);
            }
            catch (Exception)
            {
                // TODO: Add HttpException handling in place of Exception below
                throw new Exception("cart.Lines was null.");
            }

            var firstCartLineProduct = cart.Lines.FirstOrDefault();
            int establishmentId = firstCartLineProduct.Product.EstablishmentId;

            order.EstablishmentId = establishmentId;

            return order;
        }*/

        #endregion

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    context.Dispose();
                }
            }
            _disposed = true;
        }
    }
}
