using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using POS.Domain.Model;
using POS.Infrastructure;
using POS.Models;
using Telerik.Web.Mvc;
using POS.CustomExtensions;

namespace POS.Controllers
{ 
    public class OrderDetailsController : Controller
    {
        private EfDbContext db = new EfDbContext();


        public ViewResult Index()
        {
            var orderdetails = db.OrderDetails.Include(o => o.Order);
            return View(orderdetails.ToList());
        }


        public ViewResult FullOrders()
        {
            var orders = db.Orders.Include(o => o.OrderDetails);
            return View(orders);
        }

        public ActionResult DeeperLook(bool? ajax, bool? scrolling, bool? paging, bool? filtering, bool? sorting,
            bool? grouping, bool? showFooter)
        {
            //Total Sales, Gross Revenue, Line Item Promotion Total
            var model = new List<DeeperLookViewModel>();

            var orders = db.Orders.Include(o => o.OrderDetails);
            var orderTotalList = orders.Select(order => order.TotalCost).ToList();
            var orderRevenueList = orders.Select(order => order.TotalCost + order.SalesTax).ToList();
            var lineItemPromoTotalList = (from order in orders from line in order.OrderDetails select (line.UnitPrice - line.UnitPriceAfterPromo) * line.Quantity).ToList();

            var totalSales = new DeeperLookViewModel
                {
                    DeeperLookViewModelId = 1,
                    Stat = "Total Sales",
                    Average = (float) orderTotalList.Average(),
                    Median = (float) orderTotalList.Median()
                };
            model.Add(totalSales);

            var grossRevenue = new DeeperLookViewModel
                {
                    DeeperLookViewModelId = 2,
                    Stat = "Gross Revenue",
                    Average = (float) orderRevenueList.Average(),
                    Median = (float) orderRevenueList.Median()
                };
            model.Add(grossRevenue);

            var lineItemPromoTotal = new DeeperLookViewModel
                {
                    DeeperLookViewModelId = 3,
                    Stat = "Line Item Promo Total",
                    Average = (float) lineItemPromoTotalList.Average(),
                    Median = (float) lineItemPromoTotalList.Median()
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

        [GridAction]
        public ActionResult _DeeperLook()
        {
            var model = new List<DeeperLookViewModel>();

            var orders = db.Orders.Include(o => o.OrderDetails);
            var orderTotalList = orders.Select(order => order.TotalCost).ToList();
            var orderRevenueList = orders.Select(order => order.TotalCost + order.SalesTax).ToList();
            var lineItemPromoTotalList = (from order in orders from line in order.OrderDetails select (line.UnitPrice - line.UnitPriceAfterPromo) * line.Quantity).ToList();

            
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


        public ActionResult GrossRevHourly(bool? ajax, bool? scrolling, bool? paging, bool? filtering, bool? sorting,
            bool? grouping, bool? showFooter)
        {
            //GrossRevHourly
            var model = new List<DeeperLookViewModel>();

            var orders = db.Orders.Include(o => o.OrderDetails);
            var oneHourAgo = DateTime.Now.AddHours(-1.00);
            
            var ordersPlacedInTheLastHour = orders.Where(order => order.TimeProcessed > oneHourAgo).ToList();
            var ordersPlaceInTheLastHourRevenueList = ordersPlacedInTheLastHour.Select(order => order.TotalCost + order.SalesTax).ToList();

            var ordersPlacedBeforeOneHourAgo =
                orders.Where(order => order.TimeProcessed < oneHourAgo).ToList();
            var ordersPlacedBeforeOneHourAgoRevenueList = ordersPlacedBeforeOneHourAgo.Select(order => order.TotalCost + order.SalesTax).ToList();

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

        [GridAction]
        public ActionResult _DeeperStat(int id)
        {
            var model = new List<DeeperLookViewModel>();

            if (id == 1)
            {
                ///GrossSalesHourly id =1
                var orders = db.Orders.Include(o => o.OrderDetails);
                var oneHourAgo = DateTime.Now.AddHours(-1.00);

                var ordersPlacedInTheLastHour = orders.Where(order => order.TimeProcessed > oneHourAgo).ToList();
                var ordersPlacedInTheLastHourRevenueList = ordersPlacedInTheLastHour.Select(order => order.TotalCost).ToList();

                var ordersPlacedBeforeOneHourAgo =
                    orders.Where(order => order.TimeProcessed < oneHourAgo).ToList();
                var ordersPlacedBeforeOneHourAgoRevenueList = ordersPlacedBeforeOneHourAgo.Select(order => order.TotalCost).ToList();

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
            if (id == 2)
            {
                ///GrossRevHourly id =2
                var orders = db.Orders.Include(o => o.OrderDetails);
                var oneHourAgo = DateTime.Now.AddHours(-1.00);

                var ordersPlacedInTheLastHour = orders.Where(order => order.TimeProcessed > oneHourAgo).ToList();
                var ordersPlacedInTheLastHourRevenueList = ordersPlacedInTheLastHour.Select(order => order.TotalCost + order.SalesTax).ToList();

                var ordersPlacedBeforeOneHourAgo =
                    orders.Where(order => order.TimeProcessed < oneHourAgo).ToList();
                var ordersPlacedBeforeOneHourAgoRevenueList = ordersPlacedBeforeOneHourAgo.Select(order => order.TotalCost + order.SalesTax).ToList();

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
                ///TotalPromoHourly id =3
                var orders = db.Orders.Include(o => o.OrderDetails);
                var oneHourAgo = DateTime.Now.AddHours(-1.00);

                var ordersPlacedInTheLastHourRevenueList = (from order in orders
                                                            where order.TimeProcessed >= oneHourAgo
                                                            from orderDetail in order.OrderDetails
                                                            where orderDetail.LineItemPromoId != null
                                                            select (orderDetail.LineItemPromo.Promo.PercentOff*(double) orderDetail.UnitPrice)*orderDetail.Quantity).ToList();

                var ordersPlacedBeforeOneHourAgoRevenueList = (from order in orders
                                                            where order.TimeProcessed < oneHourAgo
                                                            from orderDetail in order.OrderDetails
                                                            where orderDetail.LineItemPromoId != null
                                                            select (orderDetail.LineItemPromo.Promo.PercentOff * (double)orderDetail.UnitPrice) * orderDetail.Quantity).ToList();


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


            return View(new GridModel(model));
        }


        public ActionResult Master(bool? ajax, bool? scrolling, bool? paging, bool? filtering, bool? sorting,
            bool? grouping, bool? showFooter)
        {
            var model = from o in db.OrderDetails
                        select new MasterViewModel
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
        public ActionResult _Master()
        {
            var model = from o in db.OrderDetails
                        select new MasterViewModel
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

        public ActionResult EstablishmentSalesReport(int id, bool? ajax, bool? scrolling, bool? paging, bool? filtering, bool? sorting,
    bool? grouping, bool? showFooter)
        {
            var model = from o in db.OrderDetails where o.Order.EstablishmentId == id
                        select new MasterViewModel
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
        [GridAction]
        public ActionResult _EstablishmentSalesReport(int id)
        {
            var model = from o in db.OrderDetails where o.Order.EstablishmentId == id
                        select new MasterViewModel
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


        public ViewResult Details(int id)
        {
            OrderDetail orderdetail = db.OrderDetails.Find(id);
            return View(orderdetail);
        }

        //
        // GET: /OrderDetails/Create

        public ActionResult Create()
        {
            ViewBag.OrderId = new SelectList(db.Orders, "OrderId", "OrderId");
            return View();
        } 

        //
        // POST: /OrderDetails/Create

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
        
        //
        // GET: /OrderDetails/Edit/5
 
        public ActionResult Edit(int id)
        {
            OrderDetail orderdetail = db.OrderDetails.Find(id);
            ViewBag.OrderId = new SelectList(db.Orders, "OrderId", "OrderId", orderdetail.OrderId);
            return View(orderdetail);
        }

        //
        // POST: /OrderDetails/Edit/5

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

        //
        // GET: /OrderDetails/Delete/5
 
        public ActionResult Delete(int id)
        {
            OrderDetail orderdetail = db.OrderDetails.Find(id);
            return View(orderdetail);
        }

        //
        // POST: /OrderDetails/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {            
            OrderDetail orderdetail = db.OrderDetails.Find(id);
            db.OrderDetails.Remove(orderdetail);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }

        public decimal totalCostAverage { get; set; }
    }
}