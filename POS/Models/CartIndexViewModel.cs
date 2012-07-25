using POS.Domain.Entities;

namespace POS.Models
{
    public class CartIndexViewModel
    {
        public Cart Cart { get; set; }

        public string ReturnUrl { get; set; }
    }
}