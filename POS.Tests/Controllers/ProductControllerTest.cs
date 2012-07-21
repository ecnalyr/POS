using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using POS.Controllers;
using POS.Domain.Abstract;
using POS.Domain.Entities;

namespace POS.Tests
{
    /// <summary>
    /// This is a test class for ProductControllerTest and is intended
    /// to contain all ProductControllerTest Unit Tests
    ///</summary>
    [TestClass]
    public class ProductControllerTest
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

        /// <summary>
        /// Tests that the returned PartialViewResult contains the appropriate categories for the selected parent category
        /// First checks that the number of categories is correct
        /// Then checks that the number of categories selected by a specific name is correct, to ensure that the Action did not
        /// return categories from different parent categories with the same Category.Name
        ///</summary>
        [TestMethod]
        public void ProductListReturnsAppropriateProducts()
        {
            // Arrange - create a controller
            var controller = new ProductController(_mockRepository.Object);

            // Action
            var result = (PartialViewResult) controller.ProductList(2);

            // Assert
            Assert.AreEqual(((IEnumerable<Product>) result.ViewData.Model).Count(), 2);
            Assert.IsTrue(((IEnumerable<Product>) result.ViewData.Model).Count(o => o.Name == "P4") == 1);
        }

        /// <summary>
        /// Tests that the returned PartialViewResult contains the appropriate products for the selected category
        /// First checks that the number of products is correct
        /// Then checks that the number of products selected by a specific name is correct, to ensure that the Action did not
        /// return products from different categories with the same Product.Name
        ///</summary>
        [TestMethod]
        public void CategoryListReturnsAppropriateCategories()
        {
            // Arrange - create a controller
            var controller = new ProductController(_mockRepository.Object);

            // Action
            var result = (PartialViewResult) controller.CategoryList(2);

            // Assert
            Assert.AreEqual(((IEnumerable<Category>) result.ViewData.Model).Count(), 3);
            Assert.IsTrue(((IEnumerable<Category>) result.ViewData.Model).Count(o => o.Name == "C4") == 1);
        }

        /// <summary>
        /// Tests that the returned categories ViewResult IQueryable contains every category
        ///</summary>
        [TestMethod]
        public void ListReturnsEveryCategory()
        {
            // Arrange - create a controller
            var controller = new ProductController(_mockRepository.Object);

            // Action
            var result = controller.List();

            // Assert
            Assert.AreEqual(((IQueryable<Category>)result.ViewData.Model).Count(), 5);
            Assert.IsTrue(((IQueryable<Category>)result.ViewData.Model).Count(o => o.Name == "C4") == 2);
        }

        /// <summary>
        /// Tests that the returned parent categories ViewResult IQueryable contains every parent category
        ///</summary>
        [TestMethod]
        public void ParentCategoriesReturnsEveryParentCategory()
        {
            // Arrange - create a controller
            var controller = new ProductController(_mockRepository.Object);

            // Action
            var result = (ViewResult)controller.ParentCategories();

            // Assert
            Assert.AreEqual(((IQueryable<ParentCategory>)result.ViewData.Model).Count(), 4);
            Assert.IsTrue(((IQueryable<ParentCategory>)result.ViewData.Model).Count(o => o.Name == "PC2") == 2);
        }

        /// <summary>
        /// Tests that the returned categories PartialViewResult IQueryable contains every category
        ///</summary>
        [TestMethod]
        public void CategoriesReturnsEveryCategory()
        {
            // Arrange - create a controller
            var controller = new ProductController(_mockRepository.Object);

            // Action
            var result = (PartialViewResult)controller.Categories();

            // Assert
            Assert.AreEqual(((IQueryable<Category>)result.ViewData.Model).Count(), 5);
            Assert.IsTrue(((IQueryable<Category>)result.ViewData.Model).Count(o => o.Name == "C4") == 2);
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
    }
}