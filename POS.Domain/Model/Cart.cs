using System.Collections.Generic;
using System.Linq;

namespace POS.Domain.Model
{
    /// <summary>
    /// A shopping cart
    /// </summary>
    public class Cart
    {
        private readonly List<CartLine> _lineCollection = new List<CartLine>();

        public IEnumerable<CartLine> Lines
        {
            get { return _lineCollection; }
        }

        public void AddItem(Product product, int quantity)
        {
            CartLine line = _lineCollection.FirstOrDefault(p => p.Product.ProductId == product.ProductId);
            if (line == null)
            {
                _lineCollection.Add(new CartLine { Product = product, Quantity = quantity });
            }
            else
            {
                line.Quantity += quantity;
            }
        }

        public void RemoveLine(Product product)
        {
            _lineCollection.RemoveAll(l => l.Product.ProductId == product.ProductId);
        }

        public decimal ComputeTotalValue()
        {
            return _lineCollection.Sum(e => e.Product.Price * e.Quantity);
        }

        public void Clear()
        {
            _lineCollection.Clear();
        }
    }

    /// <summary>
    /// Gets or sets a line-item within the cart.
    /// A line-item contains a product and the quantity of that product.
    /// </summary>
    public class CartLine
    {
        public Product Product { get; set; }
        public int Quantity { get; set; }
    }
}