using System.Collections.Generic;
using System.Linq;
using Moq;
using POS.Domain.Abstract;
using POS.Domain.Entities;

namespace POS.Tests.WebUi.Controllers
{
    using System.Web.Mvc;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using POS.Controllers;

    /// <summary>
    ///This is a test class for HomeController and is intended
    ///to contain all HomeController Unit Tests
    ///</summary>
    [TestClass]
    public class HomeControllerTest
    {
        #region Fields

        private Mock<IEstablishmentRepository> _mockRepository;

        #endregion

        /// <summary>
        /// Initializes a fresh _mockRepository before each test is run
        /// </summary>
        [TestInitialize]
        public void MyTestInitialize()
        {
            var mock = new Mock<IEstablishmentRepository>();
            mock.Setup(m => m.Establishments).Returns(
                new[]
                    {
                        new Establishment {EstablishmentId = 1, Name = "E1"},
                        new Establishment {EstablishmentId = 2, Name = "E2"},
                        new Establishment {EstablishmentId = 3, Name = "E3"},
                        new Establishment {EstablishmentId = 4, Name = "E4"},
                        new Establishment {EstablishmentId = 5, Name = "E4",}
                    }.AsQueryable());
            _mockRepository = mock;
        }

        /// <summary>
        /// Tests that the returned IEnumerable of Establishments contains all establishments from the repository
        /// First checks that there is the proper number of establishments, then checks that the establishment names are in their expected locations
        ///</summary>
        [TestMethod]
        public void IndexReturnsEntireEstablishmentList()
        {
            // Arrange - create a controller
            var controller = new EstablishmentManagerController(_mockRepository.Object);

            // Action
            Establishment[] result = ((IEnumerable<Establishment>)controller.Index().ViewData.Model).ToArray();

            // Assert
            Assert.AreEqual(result.Length, 5);
            Assert.AreEqual("E1", result[0].Name);
            Assert.AreEqual("E2", result[1].Name);
            Assert.AreEqual("E3", result[2].Name);
            Assert.AreEqual("E4", result[3].Name);
            Assert.AreEqual("E4", result[4].Name);
        }
    }
}
