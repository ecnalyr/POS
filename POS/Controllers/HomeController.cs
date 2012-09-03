namespace POS.Controllers
{
    #region

    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using System.Web.Mvc;

    using POS.Domain.Abstract;
    using POS.Domain.Model;

    #endregion

    public class HomeController : ControllerBase
    {
        #region Fields

        private readonly IEstablishmentRepository establishmentRepository;

        #endregion

        #region Constructors and Destructors

        public HomeController(IEstablishmentRepository establishmentRepo)
        {
            this.establishmentRepository = establishmentRepo;
        }

        #endregion

        #region Public Methods and Operators

        public ActionResult Establishment(int id)
        {
            Establishment establishment =
                this.establishmentRepository.Establishments.FirstOrDefault(p => p.EstablishmentId == id);
            return View(establishment);
        }

        public ActionResult EstablishmentProductList(int id)
        {
            Establishment establishment =
                this.establishmentRepository.Establishments.FirstOrDefault(p => p.EstablishmentId == id);
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

        public ActionResult Index()
        {
            Logger.Debug("Checking that nLogger is working from Index");
            return View(this.establishmentRepository.Establishments);
        }

        public PartialViewResult _EstablishmentSummary(int id)
        {
            Establishment establishment =
                this.establishmentRepository.Establishments.FirstOrDefault(p => p.EstablishmentId == id);
            return PartialView(establishment);
        }

        #endregion
    }
}