using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;

namespace POS.Domain.Entities
{
    public class Establishment
    {
        [HiddenInput(DisplayValue = false)]
        public int EstablishmentId { get; set; }

        public string Name { get; set; }
    }
}
