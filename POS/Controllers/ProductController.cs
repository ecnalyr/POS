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
        #region Fields

        private readonly IProductRepository _productRepository;

        #endregion

        #region Constructors and Destructors

        public ProductController(IProductRepository productRepo)
        {
            _productRepository = productRepo;
        }

        #endregion

        #region Public Methods and Operators

        public ActionResult Categories()
        {
            return PartialView(_productRepository.Categories);
        }

        public ActionResult CategoryList(int parentCategory)
        {
            IEnumerable<Category> categoryList = _productRepository.Categories.Where(p => p.ParentCategoryId == parentCategory);
            return PartialView("CategoryList", categoryList);
        }

        public FileContentResult GetImage(int productid)
        {
            Product product = _productRepository.Products.FirstOrDefault(p => p.ProductId == productid);
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
            return View(_productRepository.Categories);
        }

        [BypassAntiForgeryTokenAttribute]
        public ActionResult ParentCategories()
        {
            return View(_productRepository.ParentCategories);
        }

        [BypassAntiForgeryTokenAttribute]
        public ActionResult ProductList(int category)
        {
            IEnumerable<Product> productList = _productRepository.Products.Where(p => p.CategoryId == category);
            return PartialView("ProductList", productList);
        }

        #endregion
    }
}