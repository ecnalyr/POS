using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using POS.Domain.Model;
using POS.Infrastructure;
using POS.Models;
using Telerik.Web.Mvc;

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
    }
}