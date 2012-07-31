using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using POS.Domain.Abstract;
using POS.Domain.Model;

namespace POS.Controllers
{
    #region

    

    #endregion

    public class ProductController : ControllerBase
    {

        #region Constructors and Destructors

        public ProductController(IProductRepository productRepository) : base(productRepository)
        {
        }

        #endregion

        #region Public Methods and Operators

        public ActionResult Categories()
        {
            return PartialView(ProductRepository.Categories);
        }

        public ActionResult CategoryList(int parentCategory)
        {
            IEnumerable<Category> categoryList = ProductRepository.Categories.Where(p => p.ParentCategoryId == parentCategory);
            return PartialView("CategoryList", categoryList);
        }

        public FileContentResult GetImage(int productid)
        {
            Product product = ProductRepository.Products.FirstOrDefault(p => p.ProductId == productid);
            if (product != null)
            {
                return File(product.ImageData, product.ImageMimeType);
            }
            else
            {
                return null;
            }
        }

        public ViewResult List()
        {
            // lists everything (categories + products) for demo purposes
            return View(ProductRepository.Categories);
        }

        public ActionResult ParentCategories()
        {
            return View(ProductRepository.ParentCategories);
        }

        public ActionResult ProductList(int category)
        {
            IEnumerable<Product> productList = ProductRepository.Products.Where(p => p.CategoryId == category);
            return PartialView("ProductList", productList);
        }

        #endregion
    }
}