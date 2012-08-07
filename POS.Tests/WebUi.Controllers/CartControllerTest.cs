using System.Linq;
using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using POS.Controllers;
using POS.Domain.Abstract;
using POS.Domain.Model;
using POS.Models;

namespace POS.Tests.WebUi.Controllers
{
    /// <summary>
    ///This is a test class for CartController and is intended
    ///to contain all CartController Unit Tests
    ///</summary>
    [TestClass]
    public class CartTest
    {
        #region Public Methods and Operators

        /// <summary>
        /// Tests that the first time a given Product has been added to the cart a new CartLine is added
        /// </summary>
        [TestMethod]
        public void CanAddNewLines()
        {
            // Arrange - create some test Products
            var p1 = new Product {ProductId = 1, Name = "P1"};
            var p2 = new Product {ProductId = 2, Name = "P2"};
            // Arrange - create a new Cart
            var target = new Cart();

            // Action
            target.AddItem(p1, 1);
            target.AddItem(p2, 1);
            CartLine[] results = target.Lines.ToArray();

            // Assert
            Assert.AreEqual(results.Length, 2);
            Assert.AreEqual(results[0].Product, p1);
            Assert.AreEqual(results[1].Product, p2);
        }

        /// <summary>
        /// If an added Product is already in the Cart, the value is incremented rather than adding a new CartLine
        /// </summary>
        [TestMethod]
        public void CanAddQuantityForExistingLines()
        {
            // Arrange - create some test products
            var p1 = new Product {ProductId = 1, Name = "P1"};
            var p2 = new Product {ProductId = 2, Name = "P2"};
            // Arrange - create a new cart
            var target = new Cart();

            // Action
            target.AddItem(p1, 1);
            target.AddItem(p2, 1);
            target.AddItem(p1, 10);
            CartLine[] results = target.Lines.OrderBy(c => c.Product.ProductId).ToArray();

            // Assert
            Assert.AreEqual(results.Length, 2);
            Assert.AreEqual(results[0].Quantity, 11);
            Assert.AreEqual(results[1].Quantity, 1);
        }

        /// <summary>
        /// Tests that Products can be removed from a Cart
        /// </summary>
        [TestMethod]
        public void CanRemoveLine()
        {
            // Arrange - create some test products
            var p1 = new Product {ProductId = 1, Name = "P1"};
            var p2 = new Product {ProductId = 2, Name = "P2"};
            var p3 = new Product {ProductId = 3, Name = "P3"};
            // Arrange - create a new cart
            var target = new Cart();

            // Arrange - add some products to the cart
            target.AddItem(p1, 1);
            target.AddItem(p2, 3);
            target.AddItem(p3, 5);
            target.AddItem(p2, 1);

            // Action
            target.RemoveLine(p2);

            // Assert
            Assert.AreEqual(target.Lines.Count(c => c.Product == p2), 0);
            Assert.AreEqual(target.Lines.Count(), 2);
        }

        /// <summary>
        /// Tests that the total cost of all Products in a Cart can be calculated
        /// </summary>
        [TestMethod]
        public void CalculateCartTotal()
        {
            // Arrange - create some test products
            var p1 = new Product {ProductId = 1, Name = "P1", Price = 100M};
            var p2 = new Product {ProductId = 2, Name = "P2", Price = 50M};
            // Arrange - create a new cart
            var target = new Cart();

            // Action
            target.AddItem(p1, 1);
            target.AddItem(p2, 1);
            target.AddItem(p1, 3);
            decimal result = target.ComputeTotalValue();

            // Assert
            Assert.AreEqual(result, 450M);
        }

        /// <summary>
        /// Tests that a Cart can be reset (emptied of all Products)
        /// </summary>
        [TestMethod]
        public void CanClearContents()
        {
            // Arrange - create some test products
            var p1 = new Product {ProductId = 1, Name = "P1", Price = 100M};
            var p2 = new Product {ProductId = 2, Name = "P2", Price = 50M};
            // Arrange - create a new cart
            var target = new Cart();

            // Arrange - add some items
            target.AddItem(p1, 1);
            target.AddItem(p2, 1);

            // Action - reset the cart
            target.Clear();

            // Assert
            Assert.AreEqual(target.Lines.Count(), 0);
        }

        /// <summary>
        /// Tests that a user cannot get to checkout with Cart that has zero Products in it
        /// </summary>
        [TestMethod]
        public void CannotCheckoutEmptyCart()
        {
            // Arrange - create a mock order processor
            var mock = new Mock<IOrderProcessor>();
            // Arrange - create an empty cart
            var cart = new Cart();
            // Arrange - create shipping details
            var shippingDetails = new ShippingDetails();

            // Arrange - create an instance of the controller
            var controller = new CartController(null, mock.Object);

            // Action
            ViewResult result = controller.Checkout(cart, shippingDetails);

            // Assert - check that the order hasn't been passed on to the processor
            mock.Verify(m => m.ProcessOrder(It.IsAny<Cart>(), It.IsAny<ShippingDetails>()),
                        Times.Never());
            // Assert - check that the method is returning the default view
            Assert.AreEqual("", result.ViewName);
            // Assert - check that we are passing an invalid model to the view
            Assert.AreEqual(false, result.ViewData.ModelState.IsValid);
        }

        /// <summary>
        /// Tests that when an error is injected into the ViewModel (to simulate a problem reported by the model binder) the user cannot Checkout (i.e. when the users provides invalid shipping details)
        /// </summary>
        [TestMethod]
        public void CannotCheckoutInvalidShippingDetails()
        {
            // Arrange - create a mock order processor
            var mock = new Mock<IOrderProcessor>();
            // Arrange - create a cart with an item
            var cart = new Cart();
            cart.AddItem(new Product(), 1);
            // Arrange - create an instance of the controller
            var controller = new CartController(null, mock.Object);
            // Arrange - add an error to the model
            controller.ModelState.AddModelError("error", "error");

            // Action - try to checkout
            ViewResult result = controller.Checkout(cart, new ShippingDetails());

            // Assert - check that the order hasn't been passed on to the processor
            mock.Verify(m => m.ProcessOrder(It.IsAny<Cart>(), It.IsAny<ShippingDetails>()),
                        Times.Never());
            // Assert - check that the method is returning the default view
            Assert.AreEqual("", result.ViewName);
            // Assert - check that we are passing an invalid model to the view
            Assert.AreEqual(false, result.ViewData.ModelState.IsValid);
        }

        /// <summary>
        /// Tests that, when given an appropriate cart (one with at least one Product and no errors), a user can complete order checkout.
        /// </summary>
        [TestMethod]
        public void CanCheckoutAndSubmitOrder()
        {
            // Arrange - create a mock order processor
            var mock = new Mock<IOrderProcessor>();
            // Arrange - create a cart with an item
            var cart = new Cart();
            cart.AddItem(new Product {ProductId = 1, Name = "P1", Price = 100M, EstablishmentId = 1}, 1);
            // Arrange - create an instance of the controller
            var controller = new CartController(null, mock.Object);

            // Action - try to checkout
            ViewResult result = controller.Checkout(cart, new ShippingDetails());

            // Assert - check that the order has been passed on to the processor
            mock.Verify(m => m.ProcessOrder(It.IsAny<Cart>(), It.IsAny<ShippingDetails>()),
                        Times.Once());
            // Assert - check that the method is returning the Completed view
            Assert.AreEqual("Completed", result.ViewName);
            // Assert - check that we are passing a valid model to the view
            Assert.AreEqual(true, result.ViewData.ModelState.IsValid);
        }

        /// <summary>
        /// The URL the user can follow to return to the catalogue should be correctly passed to the Index action method
        /// </summary>
        [TestMethod]
        public void CanViewCartContents()
        {
            // Arrange - create a Cart
            var cart = new Cart();
            // Arrange - create the controller
            var target = new CartController(null, null);

            // Action - call the Index action method
            var result
                = (CartIndexViewModel) target.Index(cart, "myUrl").ViewData.Model;

            // Assert
            Assert.AreSame(result.Cart, cart);
            Assert.AreEqual(result.ReturnUrl, "myUrl");
        }

        /// <summary>
        /// After adding a Product to the cart the User should be redirected to the Index view
        /// </summary>
        [TestMethod]
        public void AddingProductToCartGoesToCartScreen()
        {
            // Arrange - create the mock repository
            var mock = new Mock<IProductRepository>();
            mock.Setup(m => m.Products).Returns(new[]
                {
                    new Product {ProductId = 1, Name = "P1", CategoryId = 1},
                }.AsQueryable());
            // Arrange - create a Cart
            var cart = new Cart();
            // Arrange - create the controller
            var target = new CartController(mock.Object, null);

            // Action - add a product to the cart
            RedirectToRouteResult result = target.AddToCart(cart, 2, "myUrl");

            // Assert
            Assert.AreEqual(result.RouteValues["action"], "Index");
            Assert.AreEqual(result.RouteValues["returnUrl"], "myUrl");
        }

        /// <summary>
        /// Tests that the selected Product is added to the Cart
        /// </summary>
        [TestMethod]
        public void CanAddToCart()
        {
            // Arrange - create the mock repository
            var mock = new Mock<IProductRepository>();
            mock.Setup(m => m.Products).Returns(new[]
                {
                    new Product {ProductId = 1, Name = "P1", CategoryId = 1},
                }.AsQueryable());
            // Arrange - create a Cart
            var cart = new Cart();
            // Arrange - create the controller
            var target = new CartController(mock.Object, null);

            // Action - add a product to the cart
            target.AddToCart(cart, 1, null);

            // Assert
            Assert.AreEqual(cart.Lines.Count(), 1);
            Assert.AreEqual(cart.Lines.ToArray()[0].Product.ProductId, 1);
        }

        /// <summary>
        /// Tests that a Product with a different EstablishmentId than the current Cart._establishmentId cannot be added to the cart
        /// </summary>
        [TestMethod]
        public void CannotAddNewProductFromDifferentEstablishment()
        {
            // Arrange - create the mock repository
            var mock = new Mock<IProductRepository>();
            mock.Setup(m => m.Products).Returns(new[]
                {
                    new Product {ProductId = 1, Name = "P1", EstablishmentId = 1},
                    new Product {ProductId = 2, Name = "P2", EstablishmentId = 2}
                }.AsQueryable());
            // Arrange - create a Cart
            var cart = new Cart();
            // Arrange - create the controller
            var target = new CartController(mock.Object, null);

            // Action - add a product to the cart
            target.AddToCart(cart, 1, null);
            target.AddToCart(cart, 2, null);

            // Assert
            Assert.AreEqual(cart.Lines.Count(), 1);
            Assert.AreEqual(cart.Lines.ToArray()[0].Product.ProductId, 1);
        }

        #endregion
    }
}