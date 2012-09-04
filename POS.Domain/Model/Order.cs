using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace POS.Domain.Model
{
    public class Order
    {
        public int OrderId { get; set; }

        public List<OrderDetail> OrderDetails { get; set; }

        public int EstablishmentId { get; set; } // this is a redundant as it can be gathered by picking any one of the given products and looking at the EstablishmentId

        public virtual Establishment Establishment { get; set; }
    }

    public class OrderDetail
    {
        public int OrderDetailId { get; set; }

        public int OrderId { get; set; }

        //public int ProductId { get; set; }

        public int Quantity { get; set; }

        public string ProductName { get; set; }

        public decimal UnitPrice { get; set; }

        //public virtual Product Product { get; set; }

        public virtual Order Order { get; set; }

    }
}
