using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace POS.Domain.Model
{
    public class Order
    {
        public int OrderId { get; set; }

        public List<CartLine> CartLines { get; set; }

        public int EstablishmentId { get; set; }

        public virtual Establishment Establishment { get; set; }
    }
}
