using System.Linq;
using System.Web;
using System.Web.Mvc;
using POS.Domain.Abstract;
using POS.Domain.Model;

namespace POS.Controllers
{
    #region

    

    #endregion

    public class AdminController : ControllerBase
    {
        #region Constructors and Destructors

        public AdminController(IProductRepository productRepository)
            : base(productRepository)
        {
        }

        #endregion

        #region Public Methods and Operators

        public ViewResult Create()
        {
            return View("Edit", new Product());
        }

        public ViewResult CreateCategory()
        {
            return View("EditCategory", new Category());
        }

        public ViewResult CreateParentCategory()
        {
            return View("EditParentCategory", new ParentCategory());
        }

        [HttpPost]
        public ActionResult Delete(int id)
        {
            Product product = ProductRepository.Products.FirstOrDefault(p => p.ProductId == id);
            if (product != null)
            {
                ProductRepository.DeleteProduct(product);
                TempData["message"] = string.Format("Product {0} was deleted", product.Name);
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult DeleteCategory(int id)
        {
            Category category = ProductRepository.Categories.FirstOrDefault(p => p.CategoryId == id);
            if (category != null)
            {
                ProductRepository.DeleteCategory(category);
                TempData["message"] = string.Format("Category {0} was deleted", category.Name);
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult DeleteParentCategory(int id)
        {
            ParentCategory parentCategory = ProductRepository.ParentCategories.FirstOrDefault(p => p.ParentCategoryId == id);
            if (parentCategory != null)
            {
                ProductRepository.DeleteParentCategory(parentCategory);
                TempData["message"] = string.Format("Parent-Category {0} was deleted", parentCategory.Name);
            }

            return RedirectToAction("Index");
        }

        public ViewResult Edit(int id)
        {
            Product product = ProductRepository.Products.FirstOrDefault(p => p.ProductId == id);
            return View(product);
        }

        [HttpPost]
        public ActionResult Edit(Product product, HttpPostedFileBase image)
        {
            if (ModelState.IsValid)
            {
                if (image != null)
                {
                    product.ImageMimeType = image.ContentType;
                    product.ImageData = new byte[image.ContentLength];
                    image.InputStream.Read(product.ImageData, 0, image.ContentLength);
                }

                ProductRepository.SaveProduct(product);
                TempData["message"] = string.Format("Product {0} has been saved", product.Name);
                return RedirectToAction("Index");
            }
            else
            {
                // there is something wrong with the data values
                return View(product);
            }
        }

        public ViewResult EditCategory(int id)
        {
            Category category = ProductRepository.Categories.FirstOrDefault(p => p.CategoryId == id);
            return View(category);
        }

        [HttpPost]
        public ActionResult EditCategory(Category category)
        {
            if (ModelState.IsValid)
            {
                ProductRepository.SaveCategory(category);
                TempData["message"] = string.Format("Category {0} has been saved", category.Name);
                return RedirectToAction("IndexCategory");
            }
            else
            {
                // there is something wrong with the data values
                return View(category);
            }
        }

        public ViewResult EditParentCategory(int id)
        {
            ParentCategory parentCategory = ProductRepository.ParentCategories.FirstOrDefault(p => p.ParentCategoryId == id);
            return View(parentCategory);
        }

        [HttpPost]
        public ActionResult EditParentCategory(ParentCategory parentCategory)
        {
            if (ModelState.IsValid)
            {
                ProductRepository.SaveParentCategory(parentCategory);
                TempData["message"] = string.Format("Parent {0} has been saved", parentCategory.Name);
                return RedirectToAction("IndexParentCategory");
            }
            else
            {
                // there is something wrong with the data values
                return View(parentCategory);
            }
        }

        public ViewResult Index()
        {
            return View(ProductRepository.Products);
        }

        public ViewResult IndexCategory()
        {
            return View(ProductRepository.Categories);
        }

        public ViewResult IndexParentCategory()
        {
            return View(ProductRepository.ParentCategories);
        }

        #endregion
    }
}