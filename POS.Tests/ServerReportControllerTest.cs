using POS.Controllers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace POS.Tests
{
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using System.Web.Mvc;

    using Moq;

    using POS.Domain.Abstract;
    using POS.Domain.Model;
    using POS.Models;

    /// <summary>
    ///This is a test class for ServerReportControllerTest and is intended
    ///to contain all ServerReportControllerTest Unit Tests
    ///</summary>
    [TestClass]
    public class ServerReportControllerTest
    {

        #region Fields

        private Mock<IOrderProcessor> _mockRepository;

        #endregion

        [TestInitialize]
        public void MyTestInitialize()
        {
            var mock = new Mock<IOrderProcessor>();
            mock.Setup(m => m.Orders).Returns(
                new[]
                    {
                        new Order
                            {
                                OrderDetails =
                                    new List<OrderDetail>()
                                        {
                                            new OrderDetail
                                                {
                                                    OrderId = 1,
                                                    Quantity = 2,
                                                    ProductName = "Basketball",
                                                    UnitPrice = 14,
                                                    LineItemPromoId = 2
                                                },
                                            new OrderDetail
                                                {
                                                    OrderId = 1,
                                                    Quantity = 1,
                                                    ProductName = "Tennis Racket",
                                                    UnitPrice = 47,
                                                    LineItemPromoId = 1
                                                },
                                            new OrderDetail
                                                {
                                                    OrderId = 1,
                                                    Quantity = 3,
                                                    ProductName = "Tennis Ball",
                                                    UnitPrice = 6
                                                }
                                        },
                                EstablishmentId = 1,
                                TotalCost = (decimal)62.5,
                                SalesTax = (decimal)5.15625,
                                CustomerName = "Albert",
                                ServerId = 2,
                                ServerTip = (decimal)1.50,
                                TimeProcessed = DateTime.Now.AddHours(-3.00)
                            },
                        new Order
                            {
                                OrderDetails =
                                    new List<OrderDetail>()
                                        {
                                            new OrderDetail
                                                {
                                                    OrderId = 2,
                                                    Quantity = 2,
                                                    ProductName = "Basketball",
                                                    UnitPrice = 14
                                                },
                                            new OrderDetail
                                                {
                                                    OrderId = 2,
                                                    Quantity = 1,
                                                    ProductName = "Tennis Racket",
                                                    UnitPrice = 47
                                                },
                                            new OrderDetail
                                                {
                                                    OrderId = 2,
                                                    Quantity = 3,
                                                    ProductName = "Tennis Ball",
                                                    UnitPrice = 6
                                                },
                                        },
                                EstablishmentId = 1,
                                TotalCost = 93,
                                SalesTax = (decimal)7.6725,
                                CustomerName = "Henry",
                                ServerId = 3,
                                ServerTip = (decimal)2.25,
                                TimeProcessed = DateTime.Now
                            },
                        new Order
                            {
                                OrderDetails =
                                    new List<OrderDetail>()
                                        {
                                            new OrderDetail
                                                {
                                                    OrderId = 3,
                                                    Quantity = 1,
                                                    ProductName = "Basketball",
                                                    UnitPrice = 14
                                                }
                                        },
                                EstablishmentId = 1,
                                TotalCost = 14,
                                SalesTax = (decimal)1.155,
                                CustomerName = "Thomas",
                                ServerId = 4,
                                ServerTip = (decimal)3.76,
                                TimeProcessed = DateTime.Now.AddHours(-0.50)
                            },
                    }.AsQueryable());
            _mockRepository = mock;
        }

        /// I need total tips for the day
        /// I need Average Tips / Server
        /// Average Transactions / Server
        /// Number of Servers for time period

        /// <summary>
        /// Index calculates the total amount of tips for for all orders
        /// </summary>
        [TestMethod]
        public void TotalAmountOfTipsForAllOrders()
        {
            // Arrange
            var controller = new ServerReportController(_mockRepository.Object);
            ServerReportIndexViewModel test = new ServerReportIndexViewModel{TotalTips = (decimal)7.51};
            // Action
            ViewResult result = controller.Index();
            var viewModel = (ServerReportIndexViewModel)result.ViewData.Model;
            // Assert
            Assert.AreEqual(test.TotalTips, viewModel.TotalTips);
        }

    }
}
