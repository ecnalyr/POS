using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace POS.Domain.Model
{
    /// <summary>
    /// A shopping cart.
    /// Only items from a single Establishment can be added to a cart at one time
    /// </summary>
    public class Cart
    {
        private readonly List<CartLine> _lineCollection = new List<CartLine>();

        /// <summary>
        /// The EstablishmentID of the Products contained within the Cart
        /// </summary>
        private int _establishmentId;

        public IEnumerable<CartLine> Lines
        {
            get { return _lineCollection; }
        }

        /// <summary>
        /// Adds a CartLine to the cart.  The CartLine will contain a product with the appropriate quantity.
        /// The first CartLine added to a cart dictates the cart's _establishmentId.
        /// </summary>
        /// <param name="product">The product entity being added to the cart</param>
        /// <param name="quantity">The quantity of the product being added to the cart</param>
        public void AddItem(Product product, int quantity)
        {
            if (_establishmentId == 0) _establishmentId = product.EstablishmentId;

            if (product.EstablishmentId != _establishmentId)
            {
                // TODO: Add HttpException handling in place of Exception below
                throw new Exception("Product cannot go in cart because it does not match the establishment that other objects in this cart have");
            }

            CartLine line = _lineCollection.FirstOrDefault(p => p.Product.ProductId == product.ProductId);
            if (line == null)
            {
                _lineCollection.Add(new CartLine {Product = product, Quantity = quantity});
            }
            else
            {
                line.Quantity += quantity;
            }
        }

        /// <summary>
        /// Removes an entire CartLine
        /// </summary>
        /// <param name="product">The Product entity that is being removed from the cart (any given product has only one CartLine)</param>
        public void RemoveLine(Product product)
        {
            _lineCollection.RemoveAll(l => l.Product.ProductId == product.ProductId);

            CartLine nullIfCartIsEmpty = _lineCollection.FirstOrDefault();
            if (nullIfCartIsEmpty == null) ResetCartEstablishmentId();
        }


        public decimal ComputeTotalValue()
        {
            return _lineCollection.Sum(e => e.Product.Price*e.Quantity);
        }

        public void Clear()
        {
            _lineCollection.Clear();
            ResetCartEstablishmentId();
        }

        private void ResetCartEstablishmentId()
        {
            _establishmentId = 0;
        }
    }

    /// <summary>
    /// Gets or sets a line-item within the cart.
    /// A line-item contains a product and the quantity of that product.
    /// </summary>
    public class CartLine
    {
        public int CartLineId { get; set; }

        public Product Product { get; set; }

        public int Quantity { get; set; }

        public LineItemPromo LineItemPromo { get; set; }
    }

    /// <summary>
    /// Apply a promotion to a line item.  These promos only offer a set percent off the line.
    /// </summary>
    public class LineItemPromo
    {
        public int LineItemPromoId { get; set; }

        public int? PromoId { get; set; }

        public virtual Promo Promo { get; set; }
    }

    /// <summary>
    /// Apply a promotion to a receipt.  This promo can be used toward a receipt total.
    /// </summary>
    public class Promo
    {
        public int PromoId { get; set; }

        public string Description { get; set; }

        public float PercentOff { get; set; }
    }
}