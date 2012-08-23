using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using POS.Domain.Model;
using POS.Infrastructure;

namespace POS.Controllers
{ 
    public class OrderDetailsController : Controller
    {
        private EfDbContext db = new EfDbContext();

        //
        // GET: /OrderDetails/

        public ViewResult Index()
        {
            var orderdetails = db.OrderDetails.Include(o => o.Order);
            return View(orderdetails.ToList());
        }

        //
        // GET: /OrderDetails/Details/5

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