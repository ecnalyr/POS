using POS.Controllers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Microsoft.VisualStudio.TestTools.UnitTesting.Web;
using POS.Domain.Abstract;
using System.Web.Mvc;

namespace POS.Tests
{
    using System.Collections.Generic;
    using System.Linq;

    using Moq;

    using POS.Domain.Entities;

    /// <summary>
    ///This is a test class for AdminControllerTest and is intended
    ///to contain all AdminControllerTest Unit Tests
    ///</summary>
    [TestClass]
    public class AdminControllerTest
    {
        private Mock<IProductRepository> _mockRepository;

        /// <summary>
        /// Initializes a fresh _mockRepository before each test is run
        /// </summary>
        [TestInitialize]
        public void MyTestInitialize()
        {
            var mock = new Mock<IProductRepository>();
            mock.Setup(m => m.Products).Returns(new[]
                                                    {
                                                        new Product {ProductId = 1, Name = "P1", CategoryId = 1},
                                                        new Product {ProductId = 2, Name = "P2", CategoryId = 2},
                                                        new Product {ProductId = 3, Name = "P3", CategoryId = 1},
                                                        new Product {ProductId = 4, Name = "P4", CategoryId = 2},
                                                        new Product {ProductId = 5, Name = "P4", CategoryId = 3}
                                                    }.AsQueryable());

            mock.Setup(m => m.Categories).Returns(new[]
                                                      {
                                                          new Category
                                                              {CategoryId = 1, Name = "C1", ParentCategoryId = 1},
                                                          new Category
                                                              {CategoryId = 2, Name = "C2", ParentCategoryId = 2},
                                                          new Category
                                                              {CategoryId = 3, Name = "C3", ParentCategoryId = 2},
                                                          new Category
                                                              {CategoryId = 4, Name = "C4", ParentCategoryId = 2},
                                                          new Category
                                                              {CategoryId = 5, Name = "C4", ParentCategoryId = 1},
                                                      }.AsQueryable());

            mock.Setup(m => m.ParentCategories).Returns(new[]
                                                            {
                                                                new ParentCategory {ParentCategoryId = 1, Name = "PC1"},
                                                                new ParentCategory {ParentCategoryId = 2, Name = "PC2"},
                                                                new ParentCategory {ParentCategoryId = 3, Name = "PC2"},
                                                                new ParentCategory {ParentCategoryId = 4, Name = "PC4"}
                                                            }.AsQueryable());
            _mockRepository = mock;
        }

        #region Additional test attributes
        // 
        //You can use the following additional attributes as you write your tests:
        //
        //Use ClassInitialize to run code before running the first test in the class
        //[ClassInitialize()]
        //public static void MyClassInitialize(TestContext testContext)
        //{
        //}
        //
        //Use ClassCleanup to run code after all tests in a class have run
        //[ClassCleanup()]
        //public static void MyClassCleanup()
        //{
        //}
        //
        //Use TestInitialize to run code before running each test
        //[TestInitialize()]
        //public void MyTestInitialize()
        //{
        //}
        //
        //Use TestCleanup to run code after each test has run
        //[TestCleanup()]
        //public void MyTestCleanup()
        //{
        //}
        //
        #endregion


        /// <summary>
        /// Tests that the returned IEnumerable of Product contains all products from the repository
        /// First checks that there is the proper number of products, then checks that the product names are in their expected locations
        ///</summary>
        [TestMethod]
        public void IndexReturnsEntirProductList()
        {
            // Arrange - create a controller
            var controller = new AdminController(_mockRepository.Object);

            // Action
            var result = ((IEnumerable<Product>)controller.Index().ViewData.Model).ToArray();

            // Assert
            Assert.AreEqual(result.Length, 5);
            Assert.AreEqual("P1", result[0].Name);
            Assert.AreEqual("P2", result[1].Name);
            Assert.AreEqual("P3", result[2].Name);
            Assert.AreEqual("P4", result[3].Name);
            Assert.AreEqual("P4", result[4].Name);
        }

        /// <summary>
        /// Edit returns the appropriate product when given a valid Id value
        ///</summary>
        [TestMethod]
        public void CanEditProduct()
        {
            // Arrange - create a controller
            var controller = new AdminController(_mockRepository.Object);

            // Action
            var p1 = controller.Edit(1).ViewData.Model as Product;
            var p2 = controller.Edit(2).ViewData.Model as Product;
            var p3 = controller.Edit(3).ViewData.Model as Product;


            // Assert
            Assert.AreEqual(1, p1.ProductId);
            Assert.AreEqual(2, p2.ProductId);
            Assert.AreEqual(3, p3.ProductId);
        }

        /// <summary>
        /// Edit does not return a product when given an invalid Id value - one that is not in the repository
        ///</summary>
        [TestMethod]
        public void CannotEditNonexistentProduct()
        {
            // Arrange - create a controller
            var controller = new AdminController(_mockRepository.Object);

            // Action
            var result = (Product)controller.Edit(6).ViewData.Model;


            // Assert
            Assert.IsNull(result);
        }

        /// <summary>
        /// Tests that valid updates to the product object that the model binder has created are passed to the product repository to be saved
        /// </summary
        [TestMethod]
        public void CanSaveValidChanges()
        {
            // Arrange - create a controller
            var controller = new AdminController(_mockRepository.Object);
            // Arrange - create a product
            Product product = new Product { Name = "Test" };

            // Action - try to save the product
            ActionResult result = controller.Edit(product);

            // Assert - check that the repository was called
            _mockRepository.Verify(m => m.SaveProduct(product));
            // Assert - check the method result type
            Assert.IsNotInstanceOfType(result, typeof(ViewResult));
        }

        /// <summary>
        /// Tests that invalid updates are not passed to the repository
        /// </summary
        [TestMethod]
        public void CannotSaveInvalidChanges()
        {
            // Arrange - create a controller
            var controller = new AdminController(_mockRepository.Object);
            // Arrange - create a product
            Product product = new Product { Name = "Test" };
            // Arrange - add an error to the model state
            controller.ModelState.AddModelError("error", "error");

            // Action - try to save the product
            ActionResult result = controller.Edit(product);

            // Assert - check that the repository was called
            _mockRepository.Verify(m => m.SaveProduct(It.IsAny<Product>()), Times.Never());
            // Assert - check the method result type
            Assert.IsInstanceOfType(result, typeof(ViewResult));
        }

        /// <summary>
        /// Tests that when a valid ProductId is passed as a parameter the action method calls the DeleteProduct method of the repository and passes the correct Product object to be deleted
        /// </summary
        [TestMethod]
        public void CanDeleteValidProducts()
        {
            // Arrange - create a product
            Product product = new Product { ProductId = 2, Name = "Test" };

            // Arrange - create a local mock repository
            var localMock = new Mock<IProductRepository>();
            localMock.Setup(m => m.Products).Returns(new Product[]
                {
                    new Product {ProductId = 1, Name = "P1"},
                    product,
                    new Product {ProductId = 3, Name = "P3"}
                }.AsQueryable());

            // Arrange - create a controller
            var controller = new AdminController(localMock.Object);

            // Action - delete the product
            controller.Delete(product.ProductId);

            // assert - ensure that the repository delete method was called with the correct Product
            localMock.Verify(m => m.DeleteProduct(product));
        }

        /// <summary>
        /// Tests that when an invalid ProductId value is passed to the Delete method therepository DeleteProduct method is not called
        /// </summary
        [TestMethod]
        public void CannotDeleteInvalidProducts()
        {
            // Arrange - create a controller
            var controller = new AdminController(_mockRepository.Object);

            // Action - attempt to delete using a ProductId that does not exist
            controller.Delete(95);

            // assert - ensure that the repository delete method was not called
            _mockRepository.Verify(m => m.DeleteProduct(It.IsAny<Product>()), Times.Never());
        }
    }
}
