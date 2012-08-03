using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using POS.Domain.Model;

namespace POS.Models
{
    public class EstablishmentSummaryViewModel
    {
        public Establishment Establishment { get; set; }

        public  ICollection<ParentCategory> ParentCategories { get; set; }
    }
}