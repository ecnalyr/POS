using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace POS.Controllers
{
    using POS.Domain.Abstract;
    using POS.Domain.Entities;

    public class AdminController : Controller
    {
        private IProductRepository repository;

        public AdminController(IProductRepository productRepository)
        {
            repository = productRepository;
        }

        public ViewResult Index()
        {
            return this.View(repository.Products);
        }

        public ViewResult Edit(int id)
        {
            Product product = repository.Products.FirstOrDefault(p => p.ProductId == id);
            return this.View(product);
        }

        [HttpPost]
        public ActionResult Edit(Product product)
        {
            if (ModelState.IsValid)
            {
                repository.SaveProduct(product);
                TempData["message"] = string.Format("{0} has been saved", product.Name);
                return RedirectToAction("Index");
            }
            else
            {
                // there is something wrong with the data values
                return this.View(product);
            }
        }

        public ViewResult Create()
        {
            return this.View("Edit", new Product());
        }

        [HttpPost]
        public ActionResult Delete(int id)
        {
            Product product = repository.Products.FirstOrDefault(p => p.ProductId == id);
            if (product != null)
            {
                repository.DeleteProduct(product);
                TempData["message"] = string.Format("{0} was deleted", product.Name);
            }
            return RedirectToAction("Index");
        }
    }
}
