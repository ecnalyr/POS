using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web.Mvc;
using POS.Domain.Abstract;
using POS.Domain.Model;

namespace POS.Controllers
{
    public class HomeController : ControllerBase
    {
        #region Fields

        private readonly IEstablishmentRepository _establishmentRepository;

        #endregion

        #region Constructors and Destructors

        public HomeController(IEstablishmentRepository establishmentRepo)
        {
            _establishmentRepository = establishmentRepo;
        }

        #endregion

        public ActionResult Index()
        {
            Logger.Debug("Checking that nLogger is working from Index");
            return View(_establishmentRepository.Establishments);
        }

        public ActionResult EstablishmentProductList(int id)
        {
            Establishment establishment =
                _establishmentRepository.Establishments.FirstOrDefault(p => p.EstablishmentId == id);
            if (establishment != null)
            {
                Debug.Write(establishment.Name + "<-- that was the establishment name. ");
                ICollection<Product> products = establishment.Products;
                foreach (Product item in establishment.Products)
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
        }

        public PartialViewResult _EstablishmentSummary(int id)
        {
            Establishment establishment =
                _establishmentRepository.Establishments.FirstOrDefault(p => p.EstablishmentId == id);
            return PartialView(establishment);
        }

        public ActionResult Establishment(int id)
        {
            Establishment establishment =
                _establishmentRepository.Establishments.FirstOrDefault(p => p.EstablishmentId == id);
            return View(establishment);
        }
    }
}