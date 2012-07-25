using System.Linq;
using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using POS.Controllers;
using POS.Domain.Abstract;
using POS.Domain.Entities;
using POS.Models;

namespace POS.Tests
{
    /// <summary>
    ///This is a test class for CartControllerTest and is intended
    ///to contain all CartControllerTest Unit Tests
    ///</summary>
    [TestClass]
    public class CartControllerTest
    {
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
        /// The URL the user can follow to return to the catalogue should be correctly passed to the Index action method
        /// </summary>
        [TestMethod]
        public void Can_View_Cart_Contents()
        {
            // Arrange - create a Cart
            var cart = new Cart();
            // Arrange - create the controller
            var target = new CartController(null, null);
            
            // Action - call the Index action method
            CartIndexViewModel result
                = (CartIndexViewModel)target.Index(cart, "myUrl").ViewData.Model;
            
            // Assert
            Assert.AreSame(result.Cart, cart);
            Assert.AreEqual(result.ReturnUrl, "myUrl");
        }
    }
}