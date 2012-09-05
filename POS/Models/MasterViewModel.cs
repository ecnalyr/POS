using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace POS.Models
{
    public class MasterViewModel
    {
        public int OrderId { get; set; }

        public string ProductName { get; set; }

        public decimal Price { get; set; }

        public int ProductQuantity { get; set; }

        public string EstablishmentName { get; set; }
    }
}