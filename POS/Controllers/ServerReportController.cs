using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace POS.Controllers
{
    using POS.Domain.Abstract;
    using POS.Models;

    public class ServerReportController : ControllerBase
    {
        #region Fields

        private readonly IOrderProcessor _orderProcessor;

        #endregion

        #region Constructors and Destructors

        public ServerReportController(IOrderProcessor orderProcessor)
        {
            _orderProcessor = orderProcessor;
        }

        #endregion

        public ViewResult Index()
        {
            var orders = _orderProcessor.Orders.ToList(); // TODO: Change to use generic repository
            var tipTotal = orders.Sum(order => order.ServerTip);

            var model = new ServerReportIndexViewModel { TotalTips = tipTotal };
            return View(model);
        }

    }
}
