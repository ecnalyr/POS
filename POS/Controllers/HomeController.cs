using System.Diagnostics;
using System.Web.Mvc;
using POS.Domain.Abstract;
using POS.Domain.Model;
using System.Linq;

namespace POS.Controllers
{
    public class HomeController : ControllerBase
    {
        public HomeController(IEstablishmentRepository establishmentRepo) : base(establishmentRepo)
        {
        }

        public ActionResult Index()
        {
            Logger.Debug("Checking that nLogger is working from Index");
            return View(EstablishmentRepository.Establishments);
        }

        public ActionResult EstablishmentProductList(int id)
        {
            Establishment establishment = EstablishmentRepository.Establishments.FirstOrDefault(p => p.EstablishmentId == id);
            if (establishment != null)
            {
                Debug.Write(establishment.Name + "<-- that was the establishment name. ");
                var products = establishment.Products;
                foreach (var item in establishment.Products)
                {
                    Debug.Write(item.Name);
                }

                    return View(products);
            }
            else
            {
                // throw an error?
                return View();
            }
            return View();
        }
    }
}