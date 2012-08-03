using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using POS.Controllers;
using POS.Domain.Abstract;
using POS.Domain.Model;

namespace POS.Tests
{
    /// <summary>
    ///This is a test class for EstablishmentManagerControllerTest and is intended
    ///to contain all EstablishmentManagerControllerTest Unit Tests
    ///</summary>
    [TestClass]
    public class EstablishmentManagerControllerTest
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
            var controller = new EstablishmentController(_mockRepository.Object);

            // Action
            Establishment[] result = ((IEnumerable<Establishment>) controller.Index().ViewData.Model).ToArray();

            // Assert
            Assert.AreEqual(result.Length, 5);
            Assert.AreEqual("E1", result[0].Name);
            Assert.AreEqual("E2", result[1].Name);
            Assert.AreEqual("E3", result[2].Name);
            Assert.AreEqual("E4", result[3].Name);
            Assert.AreEqual("E4", result[4].Name);
        }

        /// <summary>
        /// Edit returns the appropriate establishment when given a valid Id value
        ///</summary>
        [TestMethod]
        public void CanEditEstablishment()
        {
            // Arrange - create a controller
            var controller = new EstablishmentController(_mockRepository.Object);

            // Action
            var e1 = controller.Edit(1).ViewData.Model as Establishment;
            var e2 = controller.Edit(2).ViewData.Model as Establishment;
            var e3 = controller.Edit(3).ViewData.Model as Establishment;

            // Assert
            Assert.AreEqual(1, e1.EstablishmentId);
            Assert.AreEqual(2, e2.EstablishmentId);
            Assert.AreEqual(3, e3.EstablishmentId);
        }

        /// <summary>
        /// Edit does not return an establishment when given an invalid Id value - one that is not in the repository
        ///</summary>
        [TestMethod]
        public void CannotEditNonexistentProduct()
        {
            // Arrange - create a controller
            var controller = new EstablishmentController(_mockRepository.Object);

            // Action
            var result = (Establishment) controller.Edit(6).ViewData.Model;

            // Assert
            Assert.IsNull(result);
        }

        /// <summary>
        /// Tests that valid updates to the establishment object that the model binder has created are passed to the establishment repository to be saved
        /// </summary
        [TestMethod]
        public void CanSaveValidChanges()
        {
            // Arrange - create a controller
            var controller = new EstablishmentController(_mockRepository.Object);
            // Arrange - create a product
            var establishment = new Establishment {Name = "Test"};

            // Action - try to save the establishment
            ActionResult result = controller.Edit(establishment, null);

            // Assert - check that the repository was called
            _mockRepository.Verify(m => m.SaveEstablishment(establishment));
            // Assert - check the method result type
            Assert.IsNotInstanceOfType(result, typeof (ViewResult));
        }

        /// <summary>
        /// Tests that invalid updates are not passed to the repository
        /// </summary
        [TestMethod]
        public void CannotSaveInvalidChanges()
        {
            // Arrange - create a controller
            var controller = new EstablishmentController(_mockRepository.Object);
            // Arrange - create a product
            var establishment = new Establishment {Name = "Test"};
            // Arrange - add an error to the model state
            controller.ModelState.AddModelError("error", "error");

            // Action - try to save the product
            ActionResult result = controller.Edit(establishment, null);

            // Assert - check that the repository was called
            _mockRepository.Verify(m => m.SaveEstablishment(It.IsAny<Establishment>()), Times.Never());
            // Assert - check the method result type
            Assert.IsInstanceOfType(result, typeof (ViewResult));
        }

        /// <summary>
        /// Tests that when a valid EstablishmentId is passed as a parameter the action method calls the DeleteEstablishment method of the repository and passes the correct Establishment object to be deleted
        /// </summary
        [TestMethod]
        public void CanDeleteValidEstablishments()
        {
            // Arrange - create a establishment
            var establishment = new Establishment {EstablishmentId = 2, Name = "Test"};

            // Arrange - create a local mock repository
            var localMock = new Mock<IEstablishmentRepository>();
            localMock.Setup(m => m.Establishments).Returns(new[]
                {
                    new Establishment {EstablishmentId = 1, Name = "P1"},
                    establishment,
                    new Establishment {EstablishmentId = 3, Name = "P3"}
                }.AsQueryable());

            // Arrange - create a controller
            var controller = new EstablishmentController(localMock.Object);

            // Action - delete the product
            controller.Delete(establishment.EstablishmentId);

            // assert - ensure that the repository Delete method was called with the correct Product
            localMock.Verify(m => m.DeleteEstablishment(establishment));
        }

        /// <summary>
        /// Tests that when an invalid EstablishmentId value is passed to the Delete method the repository DeleteEstablishment method is not called
        /// </summary
        [TestMethod]
        public void CannotDeleteInvalidEstablishments()
        {
            // Arrange - create a controller
            var controller = new EstablishmentController(_mockRepository.Object);

            // Action - attempt to delete using a EstablishmentId that does not exist
            controller.Delete(95);

            // assert - ensure that the repository Delete method was not called
            _mockRepository.Verify(m => m.DeleteEstablishment(It.IsAny<Establishment>()), Times.Never());
        }
    }
}