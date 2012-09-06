using System;

namespace POS.Models
{
    public class MasterViewModel
    {
        public int OrderId { get; set; }

        public string ProductName { get; set; }

        public decimal Price { get; set; }

        public int ProductQuantity { get; set; }

        public string EstablishmentName { get; set; }

        public decimal TotalCostOfOrder { get; set; }

        public DateTime TimeProcessed { get; set; }

        public string CustomerName { get; set; }
    }
}