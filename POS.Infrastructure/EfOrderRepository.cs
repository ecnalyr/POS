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
            var order = ProcessTheOrder(cart, shippingDetails);
            context.Orders.Add(order);
            //foreach (var item in order.OrderDetails)
            //{
            //    context.OrderDetails.Add(item);
            //}
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

        private Order ProcessTheOrder(Cart cart, ShippingDetails shippingDetails)
        {
            var order = new Order();
            var orderDetailsList = new List<OrderDetail>();

            try
            {
                foreach (var item in cart.Lines)
                {
                    var orderDetail = new OrderDetail
                        {
                            OrderId = order.OrderId,
                            ProductName = item.Product.Name,
                            Quantity = item.Quantity,
                            UnitPrice = item.Product.Price
                        };
                    // I could update the order's total cost here if I wanted
                    orderDetailsList.Add(orderDetail);
                    context.OrderDetails.Add(orderDetail);
                }
            }
            catch (Exception)
            {
                // TODO: Add HttpException handling in place of Exception below
                throw new Exception("Erorr building list of roder details -> cart.Lines was probably null.");
            }

            var firstCartLineProduct = cart.Lines.FirstOrDefault();
            int establishmentId = firstCartLineProduct.Product.EstablishmentId;

            order.OrderDetails = orderDetailsList;
            order.EstablishmentId = establishmentId;
            return order;
        }

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
