using System.Web.Mvc;
using POS.Domain.Abstract;

namespace POS.Controllers
{
    public class LocationManagerController : MasterController
    {
        public LocationManagerController(IProductRepository productRepository) : base(productRepository)
        {
        }

        public ActionResult Index()
        {
            return View();
        }

    }
}
