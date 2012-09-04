using System;
using System.Data;
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

        public void CreateOrder(Order order)
        {
            if (order.OrderId == 0)
            {
                foreach (var item in order.OrderDetails)
                {
                    context.OrderDetails.Add(item);
                }
                context.Orders.Add(order);
            }
            else
            {
                foreach (var item in order.OrderDetails)
                {
                    context.Entry(item).State = EntityState.Modified;
                }
                context.Entry(order).State = EntityState.Modified;
            }

            context.SaveChanges();
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
