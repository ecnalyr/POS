namespace POS.Controllers
{
    #region

    using System.Linq;
    using System.Web;
    using System.Web.Mvc;

    using POS.Domain.Abstract;
    using POS.Domain.Entities;

    #endregion

    public class AdminController : Controller
    {
        #region Fields

        private readonly IProductRepository repository;

        #endregion

        #region Constructors and Destructors

        public AdminController(IProductRepository productRepository)
        {
            repository = productRepository;
        }

        #endregion

        #region Public Methods and Operators

        public ViewResult Create()
        {
            return this.View("Edit", new Product());
        }

        public ViewResult CreateCategory()
        {
            return this.View("EditCategory", new Category());
        }

        public ViewResult CreateParentCategory()
        {
            return this.View("EditParentCategory", new ParentCategory());
        }

        [HttpPost]
        public ActionResult Delete(int id)
        {
            Product product = repository.Products.FirstOrDefault(p => p.ProductId == id);
            if (product != null)
            {
                repository.DeleteProduct(product);
                TempData["message"] = string.Format("Product {0} was deleted", product.Name);
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult DeleteCategory(int id)
        {
            Category category = repository.Categories.FirstOrDefault(p => p.CategoryId == id);
            if (category != null)
            {
                repository.DeleteCategory(category);
                TempData["message"] = string.Format("Category {0} was deleted", category.Name);
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult DeleteParentCategory(int id)
        {
            ParentCategory parentCategory = repository.ParentCategories.FirstOrDefault(p => p.ParentCategoryId == id);
            if (parentCategory != null)
            {
                repository.DeleteParentCategory(parentCategory);
                TempData["message"] = string.Format("Parent-Category {0} was deleted", parentCategory.Name);
            }

            return RedirectToAction("Index");
        }

        public ViewResult Edit(int id)
        {
            Product product = repository.Products.FirstOrDefault(p => p.ProductId == id);
            return this.View(product);
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

                repository.SaveProduct(product);
                TempData["message"] = string.Format("Product {0} has been saved", product.Name);
                return RedirectToAction("Index");
            }
            else
            {
                // there is something wrong with the data values
                return this.View(product);
            }
        }

        public ViewResult EditCategory(int id)
        {
            Category category = repository.Categories.FirstOrDefault(p => p.CategoryId == id);
            return this.View(category);
        }

        [HttpPost]
        public ActionResult EditCategory(Category category)
        {
            if (ModelState.IsValid)
            {
                repository.SaveCategory(category);
                TempData["message"] = string.Format("Category {0} has been saved", category.Name);
                return RedirectToAction("IndexCategory");
            }
            else
            {
                // there is something wrong with the data values
                return this.View(category);
            }
        }

        public ViewResult EditParentCategory(int id)
        {
            ParentCategory parentCategory = repository.ParentCategories.FirstOrDefault(p => p.ParentCategoryId == id);
            return this.View(parentCategory);
        }

        [HttpPost]
        public ActionResult EditParentCategory(ParentCategory parentCategory)
        {
            if (ModelState.IsValid)
            {
                repository.SaveParentCategory(parentCategory);
                TempData["message"] = string.Format("Parent {0} has been saved", parentCategory.Name);
                return RedirectToAction("IndexParentCategory");
            }
            else
            {
                // there is something wrong with the data values
                return this.View(parentCategory);
            }
        }

        public ViewResult Index()
        {
            return this.View(repository.Products);
        }

        public ViewResult IndexCategory()
        {
            return this.View(repository.Categories);
        }

        public ViewResult IndexParentCategory()
        {
            return this.View(repository.ParentCategories);
        }

        #endregion
    }
}