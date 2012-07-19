using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using POS.Models;
using POS.Domain.Abstract;
using POS.Domain.Entities;
using System.Diagnostics;

namespace POS.Controllers
{
    public class ProductController : Controller
    {
        private IProductRepository repository;

        public ProductController(IProductRepository productRepository)
        {
            repository = productRepository;
        }

        public ViewResult List() // lists everything (categories + products) for demo purposes
        {
            return View(repository.Categories.Distinct());
        }

        public ActionResult ParentCategories()
        {
            return View(repository.ParentCategories.Distinct());
        }

        public ActionResult Categories()
        {
            return PartialView(repository.Categories);
        }

        public ActionResult ProductList(int category)
        {
            Debug.Write("productList ");
            Debug.Write(category);
            IEnumerable<Product> productList = repository.Products.Where(p => p.CategoryId == category);
            return PartialView("ProductList", productList);
        }

        public ActionResult CategoryList(int parentCategory)
        {
            Debug.Write("categoryList ");
            Debug.Write(parentCategory);
            IEnumerable<Category> categoryList = repository.Categories.Where(p => p.ParentCategoryId == parentCategory);
            return PartialView("CategoryList", categoryList);
        }
    }
}
