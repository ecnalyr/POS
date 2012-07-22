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
        /// EditCategory returns the appropriate category when given a valid Id value
        ///</summary>
        [TestMethod]
        public void CanEditCategory()
        {
            // Arrange - create a controller
            var controller = new AdminController(_mockRepository.Object);

            // Action
            var c1 = controller.EditCategory(1).ViewData.Model as Category;
            var c2 = controller.EditCategory(2).ViewData.Model as Category;
            var c3 = controller.EditCategory(3).ViewData.Model as Category;

            // Assert
            Assert.AreEqual(1, c1.CategoryId);
            Assert.AreEqual(2, c2.CategoryId);
            Assert.AreEqual(3, c3.CategoryId);
        }

        /// <summary>
        /// EditCategory does not return a category when given an invalid Id value - one that is not in the repository
        ///</summary>
        [TestMethod]
        public void CannotEditNonexistentCategory()
        {
            // Arrange - create a controller
            var controller = new AdminController(_mockRepository.Object);

            // Action
            var result = (Category)controller.EditCategory(6).ViewData.Model;

            // Assert
            Assert.IsNull(result);
        }


        /// <summary>
        /// EditParentCategory returns the appropriate parent-category when given a valid Id value
        ///</summary>
        [TestMethod]
        public void CanEditProductCategory()
        {
            // Arrange - create a controller
            var controller = new AdminController(_mockRepository.Object);

            // Action
            var pc1 = controller.EditParentCategory(1).ViewData.Model as ParentCategory;
            var pc2 = controller.EditParentCategory(2).ViewData.Model as ParentCategory;
            var pc3 = controller.EditParentCategory(3).ViewData.Model as ParentCategory;

            // Assert
            Assert.AreEqual(1, pc1.ParentCategoryId);
            Assert.AreEqual(2, pc2.ParentCategoryId);
            Assert.AreEqual(3, pc3.ParentCategoryId);
        }

        /// <summary>
        /// EditParentCategory does not return a parent-category when given an invalid Id value - one that is not in the repository
        ///</summary>
        [TestMethod]
        public void CannotEditNonexistentParentCategory()
        {
            // Arrange - create a controller
            var controller = new AdminController(_mockRepository.Object);

            // Action
            var result = (Category)controller.EditParentCategory(6).ViewData.Model;

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
            ActionResult result = controller.Edit(product, null);

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
            ActionResult result = controller.Edit(product, null);

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

            // assert - ensure that the repository Delete method was called with the correct Product
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

            // assert - ensure that the repository Delete method was not called
            _mockRepository.Verify(m => m.DeleteProduct(It.IsAny<Product>()), Times.Never());
        }

        /// <summary>
        /// Tests that when a valid CategoryId is passed as a parameter the action method calls the DeleteCategory method of the repository and passes the correct Category object to be deleted
        /// </summary
        [TestMethod]
        public void CanDeleteValidCategories()
        {
            // Arrange - create a category
            Category category = new Category { CategoryId = 2, Name = "Test"};

            // Arrange - create a local mock repository
            var localMock = new Mock<IProductRepository>();
            localMock.Setup(m => m.Categories).Returns(new Category[]
                {
                    new Category  {CategoryId = 1, Name = "C1"},
                    category,
                    new Category  {CategoryId = 3, Name = "C3"}
                }.AsQueryable());

            // Arrange - create a controller
            var controller = new AdminController(localMock.Object);

            // Action - delete the category
            controller.DeleteCategory(category.CategoryId);

            // assert - ensure that the repository DeleteCategory method was called with the correct category
            localMock.Verify(m => m.DeleteCategory(category));
        }

        /// <summary>
        /// Tests that when an invalid CategoryId value is passed to the DeleteCategory method therepository DeleteCategory method is not called
        /// </summary
        [TestMethod]
        public void CannotDeleteInvalidCategories()
        {
            // Arrange - create a controller
            var controller = new AdminController(_mockRepository.Object);

            // Action - attempt to delete using a CategoryId that does not exist
            controller.DeleteCategory(95);

            // assert - ensure that the repository DeleteCategory method was not called
            _mockRepository.Verify(m => m.DeleteCategory(It.IsAny<Category>()), Times.Never());
        }

        /// <summary>
        /// Tests that when a valid ParentCategoryId is passed as a parameter the action method calls the DeleteParentCategory method of the repository and passes the correct ParentCategory object to be deleted
        /// </summary
        [TestMethod]
        public void CanDeleteValidParentCategories()
        {
            // Arrange - create a parentCategory
            ParentCategory parentCategory = new ParentCategory { ParentCategoryId = 2, Name = "Test" };

            // Arrange - create a local mock repository
            var localMock = new Mock<IProductRepository>();
            localMock.Setup(m => m.ParentCategories).Returns(new ParentCategory[]
                {
                    new ParentCategory {ParentCategoryId = 1, Name = "PC1"},
                    parentCategory,
                    new ParentCategory {ParentCategoryId = 3, Name = "PC3"}
                }.AsQueryable());

            // Arrange - create a controller
            var controller = new AdminController(localMock.Object);

            // Action - delete the parentCategory
            controller.DeleteParentCategory(parentCategory.ParentCategoryId);

            // assert - ensure that the repository delete method was called with the correct ParentCategory
            localMock.Verify(m => m.DeleteParentCategory(parentCategory));
        }

        /// <summary>
        /// Tests that when an invalid ParentCategoryId value is passed to the DeleteParentCategory method therepository DeleteParentCategory method is not called
        /// </summary
        [TestMethod]
        public void CannotDeleteInvalidParentCategories()
        {
            // Arrange - create a controller
            var controller = new AdminController(_mockRepository.Object);

            // Action - attempt to delete using a ParentCategoryId that does not exist
            controller.DeleteParentCategory(95);

            // assert - ensure that the repository DeleteParentCategory method was not called
            _mockRepository.Verify(m => m.DeleteParentCategory(It.IsAny<ParentCategory>()), Times.Never());
        }
    }
}
