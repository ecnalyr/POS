using System;
using System.Collections.Generic;

namespace POS.Domain.Model
{
    public class Order
    {
        public int OrderId { get; set; }

        public List<OrderDetail> OrderDetails { get; set; }

        public int EstablishmentId { get; set; }

        public virtual Establishment Establishment { get; set; }

        public decimal TotalCost { get; set; }

        public DateTime TimeProcessed { get; set; }

        public string CustomerName { get; set; }

        public Order()
        {
            TotalCost = 0;
        }
    }

    public class OrderDetail
    {
        public int OrderDetailId { get; set; }

        public int OrderId { get; set; }

        public int Quantity { get; set; }

        public string ProductName { get; set; }

        public decimal UnitPrice { get; set; }

        public virtual Order Order { get; set; }
    }
}
