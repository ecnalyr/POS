using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using POS.Domain.Abstract;
using POS.Domain.Model;

namespace POS.Domain.Concrete
{
    public class OrderProcessor
    {
        public Order ProcessTheOrder(Cart cart, ShippingDetails shippingDetails)
        {
            List<CartLine> cartLines = null;
            try
            {
                cartLines.AddRange(cart.Lines);
            }
            catch (Exception)
            {
                // TODO: Add HttpException handling in place of Exception below
                throw new Exception("cart.Lines was null.");
            }

            var firstCartLineProduct = cart.Lines.FirstOrDefault();
            int establishmentId = firstCartLineProduct.Product.EstablishmentId;

            var model = new Order
                {
                    CartLines = cartLines,
                    EstablishmentId = establishmentId
                };

            return model;
        }
    }
}
