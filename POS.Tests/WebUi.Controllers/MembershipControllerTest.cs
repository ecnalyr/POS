/*
namespace POS.Tests.WebUi.Controllers
{
    using System.Web.Security;

    using Moq;

    using POS.Controllers;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using POS.Infrastructure.Membership;
    using POS.Models;

    using System.Web.Mvc;

    using POS.Infrastructure;

    /// <summary>
    ///This is a test class for MembershipControllerTest and is intended
    ///to contain all MembershipControllerTest Unit Tests
    ///</summary>
    [TestClass]
    public class MembershipControllerTest
    {


        private Mock<EfDbContext> _mockRepository;

        /// <summary>
        /// Initializes a fresh _mockRepository before each test is run
        /// </summary>
        [TestInitialize]
        public void MyTestInitialize()
        {
            var mock = new Mock<EfDbContext>();
            _mockRepository = mock;
        }

        /// <summary>
        ///A test for LogOn
        ///</summary>
        [TestMethod]
        public void LogOnTest()
        {
            MembershipController target = new MembershipController(_mockRepository.Object);
            LogOnModel model = new LogOnModel() { UserName = "Admin", Password = "pas5word", RememberMe = false };
            string returnUrl = string.Empty; // TODO: Initialize to an appropriate value
            
            // act
            var result = (ViewResult) target.LogOn(model, returnUrl);

            // assert
            Assert.IsNotNull(result.ViewData.ModelState);
        }
    }
}
*/
