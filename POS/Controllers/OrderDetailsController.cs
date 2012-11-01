namespace POS.Controllers
{
    #region

    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.Entity;
    using System.Linq;
    using System.Web.Mvc;

    using POS.CustomExtensions;
    using POS.Domain.Model;
    using POS.Infrastructure;
    using POS.Models;

    using Telerik.Web.Mvc;

    #endregion

    public class OrderDetailsController : Controller
    {
        #region Fields

        private readonly EfDbContext db = new EfDbContext();

        #endregion

        #region Public Methods and Operators

        public ActionResult Create()
        {
            ViewBag.OrderId = new SelectList(db.Orders, "OrderId", "OrderId");
            return View();
        }

        [HttpPost]
        public ActionResult Create(OrderDetail orderdetail)
        {
            if (ModelState.IsValid)
            {
                db.OrderDetails.Add(orderdetail);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.OrderId = new SelectList(db.Orders, "OrderId", "OrderId", orderdetail.OrderId);
            return View(orderdetail);
        }

        public ActionResult DeeperLook(
            bool? ajax, bool? scrolling, bool? paging, bool? filtering, bool? sorting, bool? grouping, bool? showFooter)
        {
            // Total Sales, Gross Revenue, Line Item Promotion Total
            var model = new List<DeeperLookViewModel>();

            IQueryable<Order> orders = db.Orders.Include(o => o.OrderDetails);
            List<decimal> orderTotalList = orders.Select(order => order.TotalCost).ToList();
            List<decimal> orderRevenueList = orders.Select(order => order.TotalCost + order.SalesTax).ToList();
            List<decimal> lineItemPromoTotalList =
                (from order in orders
                 from line in order.OrderDetails
                 select (line.UnitPrice - line.UnitPriceAfterPromo) * line.Quantity).ToList();

            var totalSales = new DeeperLookViewModel
                {
                    DeeperLookViewModelId = 1, 
                    Stat = "Total Sales", 
                    Average = (float)orderTotalList.Average(), 
                    Median = (float)orderTotalList.Median()
                };
            model.Add(totalSales);

            var grossRevenue = new DeeperLookViewModel
                {
                    DeeperLookViewModelId = 2, 
                    Stat = "Gross Revenue", 
                    Average = (float)orderRevenueList.Average(), 
                    Median = (float)orderRevenueList.Median()
                };
            model.Add(grossRevenue);

            var lineItemPromoTotal = new DeeperLookViewModel
                {
                    DeeperLookViewModelId = 3, 
                    Stat = "Line Item Promo Total", 
                    Average = (float)lineItemPromoTotalList.Average(), 
                    Median = (float)lineItemPromoTotalList.Median()
                };
            model.Add(lineItemPromoTotal);

            ViewData["ajax"] = ajax ?? true;
            ViewData["scrolling"] = scrolling ?? true;
            ViewData["paging"] = paging ?? true;
            ViewData["filtering"] = filtering ?? true;
            ViewData["grouping"] = grouping ?? true;
            ViewData["sorting"] = sorting ?? true;
            ViewData["showFooter"] = showFooter ?? true;
            return View(model);
        }

        public ActionResult Delete(int id)
        {
            OrderDetail orderdetail = db.OrderDetails.Find(id);
            return View(orderdetail);
        }

        [HttpPost]
        [ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            OrderDetail orderdetail = db.OrderDetails.Find(id);
            db.OrderDetails.Remove(orderdetail);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ViewResult Details(int id)
        {
            OrderDetail orderdetail = db.OrderDetails.Find(id);
            return View(orderdetail);
        }

        public ActionResult Edit(int id)
        {
            OrderDetail orderdetail = db.OrderDetails.Find(id);
            ViewBag.OrderId = new SelectList(db.Orders, "OrderId", "OrderId", orderdetail.OrderId);
            return View(orderdetail);
        }

        [HttpPost]
        public ActionResult Edit(OrderDetail orderdetail)
        {
            if (ModelState.IsValid)
            {
                db.Entry(orderdetail).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.OrderId = new SelectList(db.Orders, "OrderId", "OrderId", orderdetail.OrderId);
            return View(orderdetail);
        }

        public ActionResult EstablishmentSalesReport(
            int id, 
            bool? ajax, 
            bool? scrolling, 
            bool? paging, 
            bool? filtering, 
            bool? sorting, 
            bool? grouping, 
            bool? showFooter)
        {
            IQueryable<MasterViewModel> model = from o in db.OrderDetails
                                                where o.Order.EstablishmentId == id
                                                select
                                                    new MasterViewModel
                                                        {
                                                            OrderId = o.Order.OrderId, 
                                                            ProductName = o.ProductName, 
                                                            Price = o.UnitPrice, 
                                                            ProductQuantity = o.Quantity, 
                                                            TotalLineCost = o.UnitPrice * o.Quantity, 
                                                            EstablishmentName = o.Order.Establishment.Name
                                                        };
            ViewData["ajax"] = ajax ?? true;
            ViewData["scrolling"] = scrolling ?? true;
            ViewData["paging"] = paging ?? true;
            ViewData["filtering"] = filtering ?? true;
            ViewData["grouping"] = grouping ?? true;
            ViewData["sorting"] = sorting ?? true;
            ViewData["showFooter"] = showFooter ?? true;
            return View(model);
        }

        public ViewResult FullOrders()
        {
            IQueryable<Order> orders = db.Orders.Include(o => o.OrderDetails);
            return View(orders);
        }

        public ActionResult GrossRevHourly(
            bool? ajax, bool? scrolling, bool? paging, bool? filtering, bool? sorting, bool? grouping, bool? showFooter)
        {
            // GrossRevHourly
            var model = new List<DeeperLookViewModel>();

            IQueryable<Order> orders = db.Orders.Include(o => o.OrderDetails);
            DateTime oneHourAgo = DateTime.Now.AddHours(-1.00);

            List<Order> ordersPlacedInTheLastHour = orders.Where(order => order.TimeProcessed > oneHourAgo).ToList();
            List<decimal> ordersPlaceInTheLastHourRevenueList =
                ordersPlacedInTheLastHour.Select(order => order.TotalCost + order.SalesTax).ToList();

            List<Order> ordersPlacedBeforeOneHourAgo = orders.Where(order => order.TimeProcessed < oneHourAgo).ToList();
            List<decimal> ordersPlacedBeforeOneHourAgoRevenueList =
                ordersPlacedBeforeOneHourAgo.Select(order => order.TotalCost + order.SalesTax).ToList();

            var totalSalesWithinLastHour = new DeeperLookViewModel
                {
                    DeeperLookViewModelId = 1, 
                    Stat = "Orders Placed In The Last Hour", 
                    Average = (float)ordersPlaceInTheLastHourRevenueList.Average(), 
                    Median = (float)ordersPlaceInTheLastHourRevenueList.Median()
                };
            model.Add(totalSalesWithinLastHour);

            var totalSalesBeforeLastHour = new DeeperLookViewModel
                {
                    DeeperLookViewModelId = 2, 
                    Stat = "Orders Placed Before The Last Hour", 
                    Average = (float)ordersPlacedBeforeOneHourAgoRevenueList.Average(), 
                    Median = (float)ordersPlacedBeforeOneHourAgoRevenueList.Median()
                };
            model.Add(totalSalesBeforeLastHour);

            ViewData["ajax"] = ajax ?? true;
            ViewData["scrolling"] = scrolling ?? true;
            ViewData["paging"] = paging ?? true;
            ViewData["filtering"] = filtering ?? true;
            ViewData["grouping"] = grouping ?? true;
            ViewData["sorting"] = sorting ?? true;
            ViewData["showFooter"] = showFooter ?? true;
            return View("DeeperLook", model);
        }

        public ViewResult Index()
        {
            IQueryable<OrderDetail> orderdetails = db.OrderDetails.Include(o => o.Order);
            return View(orderdetails.ToList());
        }

        public ActionResult Master(
            bool? ajax, bool? scrolling, bool? paging, bool? filtering, bool? sorting, bool? grouping, bool? showFooter)
        {
            IQueryable<MasterViewModel> model = from o in db.OrderDetails
                                                select
                                                    new MasterViewModel
                                                        {
                                                            OrderId = o.Order.OrderId, 
                                                            ProductName = o.ProductName, 
                                                            Price = o.UnitPrice, 
                                                            ProductQuantity = o.Quantity, 
                                                            TotalLineCost = o.UnitPrice * o.Quantity, 
                                                            EstablishmentName = o.Order.Establishment.Name, 
                                                            TotalCostOfOrder = o.Order.TotalCost, 
                                                            TimeProcessed = o.Order.TimeProcessed, 
                                                            CustomerName = o.Order.CustomerName, 
                                                            LineItemPromo = o.LineItemPromo.Promo.Description, 
                                                            TotalLineCostAfterPromo = o.UnitPriceAfterPromo
                                                        };
            ViewData["ajax"] = ajax ?? true;
            ViewData["scrolling"] = scrolling ?? true;
            ViewData["paging"] = paging ?? true;
            ViewData["filtering"] = filtering ?? true;
            ViewData["grouping"] = grouping ?? true;
            ViewData["sorting"] = sorting ?? true;
            ViewData["showFooter"] = showFooter ?? true;
            return View(model);
        }

        [GridAction]
        public ActionResult _DeeperLook()
        {
            var model = new List<DeeperLookViewModel>();

            IQueryable<Order> orders = db.Orders.Include(o => o.OrderDetails);
            List<decimal> orderTotalList = orders.Select(order => order.TotalCost).ToList();
            List<decimal> orderRevenueList = orders.Select(order => order.TotalCost + order.SalesTax).ToList();
            List<decimal> lineItemPromoTotalList =
                (from order in orders
                 from line in order.OrderDetails
                 select (line.UnitPrice - line.UnitPriceAfterPromo) * line.Quantity).ToList();

            var totalSales = new DeeperLookViewModel
                {
                    DeeperLookViewModelId = 1, 
                    Stat = "TotalSales", 
                    Average = (float)orderTotalList.Average(), 
                    Median = (float)orderTotalList.Median()
                };
            model.Add(totalSales);

            var grossRevenue = new DeeperLookViewModel
                {
                    DeeperLookViewModelId = 2, 
                    Stat = "GrossRevenue", 
                    Average = (float)orderRevenueList.Average(), 
                    Median = (float)orderRevenueList.Median()
                };
            model.Add(grossRevenue);

            var lineItemPromoTotal = new DeeperLookViewModel
                {
                    DeeperLookViewModelId = 3, 
                    Stat = "Line Item Promo Total", 
                    Average = (float)lineItemPromoTotalList.Average(), 
                    Median = (float)lineItemPromoTotalList.Median()
                };
            model.Add(lineItemPromoTotal);
            return View(new GridModel(model));
        }

        [GridAction]
        public ActionResult _DeeperStat(int id)
        {
            var model = new List<DeeperLookViewModel>();

            if (id == 1)
            {
                // GrossSalesHourly id =1
                IQueryable<Order> orders = db.Orders.Include(o => o.OrderDetails);
                DateTime oneHourAgo = DateTime.Now.AddHours(-1.00);

                List<Order> ordersPlacedInTheLastHour = orders.Where(order => order.TimeProcessed > oneHourAgo).ToList();
                List<decimal> ordersPlacedInTheLastHourGrossSalesList =
                    ordersPlacedInTheLastHour.Select(order => order.TotalCost).ToList();

                /*List<Order> ordersPlacedBeforeOneHourAgo =
                    orders.Where(order => order.TimeProcessed < oneHourAgo).ToList();
                List<decimal> ordersPlacedBeforeOneHourAgoGrossSalesList =
                    ordersPlacedBeforeOneHourAgo.Select(order => order.TotalCost).ToList();*/

                /*var totalSalesWithinLastHour = new DeeperLookViewModel
                    {
                        DeeperLookViewModelId = 1, 
                        Stat = "Orders Placed In The Last Hour", 
                        Average = (float)ordersPlacedInTheLastHourGrossSalesList.Average(), 
                        Median = (float)ordersPlacedInTheLastHourGrossSalesList.Median()
                    };
                model.Add(totalSalesWithinLastHour);

                var totalSalesBeforeLastHour = new DeeperLookViewModel
                    {
                        DeeperLookViewModelId = 2, 
                        Stat = "Orders Placed Before The Last Hour", 
                        Average = (float)ordersPlacedBeforeOneHourAgoGrossSalesList.Average(), 
                        Median = (float)ordersPlacedBeforeOneHourAgoGrossSalesList.Median()
                    };
                model.Add(totalSalesBeforeLastHour);*/

                var calendarStartOfBusiness = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 05, 00, 00, DateTimeKind.Local);
                DateTime todaysStartOfBusiness = calendarStartOfBusiness;
                if (DateTime.Now < calendarStartOfBusiness)
                {
                    todaysStartOfBusiness = calendarStartOfBusiness.AddDays(-1);
                }
                        
                DateTime endDate = todaysStartOfBusiness.AddDays(1);

                var hourId = 1;
                while (todaysStartOfBusiness != endDate)
                {
                    Console.WriteLine(todaysStartOfBusiness.ToString());

                    // this will show orders from all hours -- not just hours in the past 24 hours
                    List<Order> ordersPlacedDuringThisHour =
                    orders.Where(order => order.TimeProcessed.Hour == todaysStartOfBusiness.Hour).ToList();
                    List<decimal> ordersPlacedDuringThisHourList =
                        ordersPlacedDuringThisHour.Select(order => order.TotalCost).ToList();
                    float thisHourAverage = 0;
                    float thisHourMedian = 0;
                    if (ordersPlacedDuringThisHourList.Count() != 0)
                    {
                        thisHourAverage = (float)ordersPlacedDuringThisHourList.Average();
                    }
                    if (ordersPlacedDuringThisHourList.Count() != 0)
                    {
                        thisHourMedian = (float)ordersPlacedDuringThisHourList.Median();
                    }

                    var totalSales = new DeeperLookViewModel
                    {
                        DeeperLookViewModelId = hourId,
                        Stat = "Orders Placed at hour " + todaysStartOfBusiness.Hour,
                        Average = thisHourAverage,
                        Median = thisHourMedian
                    };
                    model.Add(totalSales);
                    todaysStartOfBusiness = todaysStartOfBusiness.AddHours(1);
                    hourId++;
                }
            }

            if (id == 2)
            {
                // GrossRevHourly id =2
                IQueryable<Order> orders = db.Orders.Include(o => o.OrderDetails);
                DateTime oneHourAgo = DateTime.Now.AddHours(-1.00);

                List<Order> ordersPlacedInTheLastHour = orders.Where(order => order.TimeProcessed > oneHourAgo).ToList();
                List<decimal> ordersPlacedInTheLastHourRevenueList =
                    ordersPlacedInTheLastHour.Select(order => order.TotalCost + order.SalesTax).ToList();

                List<Order> ordersPlacedBeforeOneHourAgo =
                    orders.Where(order => order.TimeProcessed < oneHourAgo).ToList();
                List<decimal> ordersPlacedBeforeOneHourAgoRevenueList =
                    ordersPlacedBeforeOneHourAgo.Select(order => order.TotalCost + order.SalesTax).ToList();

                var totalSalesWithinLastHour = new DeeperLookViewModel
                    {
                        DeeperLookViewModelId = 1, 
                        Stat = "Orders Placed In The Last Hour", 
                        Average = (float)ordersPlacedInTheLastHourRevenueList.Average(), 
                        Median = (float)ordersPlacedInTheLastHourRevenueList.Median()
                    };
                model.Add(totalSalesWithinLastHour);

                var totalSalesBeforeLastHour = new DeeperLookViewModel
                    {
                        DeeperLookViewModelId = 2, 
                        Stat = "Orders Placed Before The Last Hour", 
                        Average = (float)ordersPlacedBeforeOneHourAgoRevenueList.Average(), 
                        Median = (float)ordersPlacedBeforeOneHourAgoRevenueList.Median()
                    };
                model.Add(totalSalesBeforeLastHour);
            }

            if (id == 3)
            {
                // Do promo numbers
                // TotalPromoHourly id =3
                IQueryable<Order> orders = db.Orders.Include(o => o.OrderDetails);
                DateTime oneHourAgo = DateTime.Now.AddHours(-1.00);

                IEnumerable<double> ordersPlacedInTheLastHourLinePromoTotalList = (from order in orders
                                                                                   where
                                                                                       order.TimeProcessed >= oneHourAgo
                                                                                   from orderDetail in
                                                                                       order.OrderDetails
                                                                                   where
                                                                                       orderDetail.LineItemPromoId
                                                                                       != null
                                                                                   select
                                                                                       (orderDetail.LineItemPromo.Promo.
                                                                                            PercentOff
                                                                                        * (double)orderDetail.UnitPrice)
                                                                                       * orderDetail.Quantity).ToList().
                    DefaultIfEmpty();

                IEnumerable<double> ordersPlacedBeforeOneHourAgoLinePromoTotalList = (from order in orders
                                                                                      where
                                                                                          order.TimeProcessed
                                                                                          < oneHourAgo
                                                                                      from orderDetail in
                                                                                          order.OrderDetails
                                                                                      where
                                                                                          orderDetail.LineItemPromoId
                                                                                          != null
                                                                                      select
                                                                                          (orderDetail.LineItemPromo.
                                                                                               Promo.PercentOff
                                                                                           *
                                                                                           (double)orderDetail.UnitPrice)
                                                                                          * orderDetail.Quantity).ToList
                    ().DefaultIfEmpty();

                if (ordersPlacedInTheLastHourLinePromoTotalList.FirstOrDefault() != null)
                {
                    var totalSalesWithinLastHour = new DeeperLookViewModel
                        {
                            DeeperLookViewModelId = 1, 
                            Stat = "Orders Placed In The Last Hour", 
                            Average = (float)ordersPlacedInTheLastHourLinePromoTotalList.Average(), 
                            Median = (float)ordersPlacedInTheLastHourLinePromoTotalList.Median()
                        };
                    model.Add(totalSalesWithinLastHour);
                }
                else
// ReSharper disable HeuristicUnreachableCode
                {
                    var totalSalesWithinLastHour = new DeeperLookViewModel
                        {
                           DeeperLookViewModelId = 1, Stat = "Orders Placed In The Last Hour", Average = 0, Median = 0 
                        };
                    model.Add(totalSalesWithinLastHour);
                }
// ReSharper restore HeuristicUnreachableCode
                if (ordersPlacedBeforeOneHourAgoLinePromoTotalList.FirstOrDefault() != null)
                {
                    var totalSalesBeforeLastHour = new DeeperLookViewModel
                        {
                            DeeperLookViewModelId = 2, 
                            Stat = "Orders Placed Before The Last Hour", 
                            Average = (float)ordersPlacedBeforeOneHourAgoLinePromoTotalList.Average(), 
                            Median = (float)ordersPlacedBeforeOneHourAgoLinePromoTotalList.Median()
                        };
                    model.Add(totalSalesBeforeLastHour);
                }
                else
// ReSharper disable HeuristicUnreachableCode
                {
                    var totalSalesBeforeLastHour = new DeeperLookViewModel
                        {
                            DeeperLookViewModelId = 2, 
                            Stat = "Orders Placed Before The Last Hour", 
                            Average = 0, 
                            Median = 0
                        };
                    model.Add(totalSalesBeforeLastHour);
                }
// ReSharper restore HeuristicUnreachableCode
            }

            return View(new GridModel(model));
        }

        [GridAction]
        public ActionResult _EstablishmentSalesReport(int id)
        {
            IQueryable<MasterViewModel> model = from o in db.OrderDetails
                                                where o.Order.EstablishmentId == id
                                                select
                                                    new MasterViewModel
                                                        {
                                                            OrderId = o.Order.OrderId, 
                                                            ProductName = o.ProductName, 
                                                            Price = o.UnitPrice, 
                                                            ProductQuantity = o.Quantity, 
                                                            TotalLineCost = o.UnitPrice * o.Quantity, 
                                                            EstablishmentName = o.Order.Establishment.Name
                                                        };
            return View(new GridModel(model));
        }

        [GridAction]
        public ActionResult _Master()
        {
            IQueryable<MasterViewModel> model = from o in db.OrderDetails
                                                select
                                                    new MasterViewModel
                                                        {
                                                            OrderId = o.Order.OrderId, 
                                                            ProductName = o.ProductName, 
                                                            Price = o.UnitPrice, 
                                                            ProductQuantity = o.Quantity, 
                                                            TotalLineCost = o.UnitPrice * o.Quantity, 
                                                            EstablishmentName = o.Order.Establishment.Name, 
                                                            TotalCostOfOrder = o.Order.TotalCost, 
                                                            TimeProcessed = o.Order.TimeProcessed, 
                                                            CustomerName = o.Order.CustomerName, 
                                                            LineItemPromo = o.LineItemPromo.Promo.Description, 
                                                            TotalLineCostAfterPromo = o.UnitPriceAfterPromo
                                                        };
            return View(new GridModel(model));
        }

        #endregion

        #region Methods

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }

        #endregion
    }
}