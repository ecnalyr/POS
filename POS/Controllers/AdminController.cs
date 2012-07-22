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
                TempData["message"] = string.Format("{0} has been saved", product.Name);
                return RedirectToAction("Index");
            }
            else
            {
                // there is something wrong with the data values
                return this.View(product);
            }
        }

        public ViewResult Index()
        {
            return this.View(repository.Products);
        }

        #endregion
    }
}