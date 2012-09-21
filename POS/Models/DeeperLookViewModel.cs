using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace POS.Models
{
    public class DeeperLookViewModel
    {
        public int DeeperLookViewModelId { get; set; }

        public string Stat { get; set; }

        public float Average { get; set; }

        public float Median { get; set; }

        public DeeperLookViewModel()
        {
            Stat = "Stat";
            Average = 0;
            Median = 0;
        }
    }
}