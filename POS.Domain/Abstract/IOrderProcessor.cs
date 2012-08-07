using System;
using System.Linq;
using POS.Domain.Model;

namespace POS.Domain.Abstract
{
    public interface IOrderProcessor : IDisposable
    {
        #region Public Properties

        IQueryable<Order> Orders { get; }

        #endregion

        #region Public Methods and Operators

        void ProcessOrder(Cart cart, ShippingDetails shippingDetails);

        void DeleteOrder(Order order);

        void SaveOrder(Order order);

        #endregion
    }
}
