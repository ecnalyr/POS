using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Web.Mvc;
using POS.Domain.Abstract;
using POS.Domain.Model;
using System.Linq;
using POS.Models;

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
            Establishment establishment = _establishmentRepository.Establishments.FirstOrDefault(p => p.EstablishmentId == id);
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
        }

        public PartialViewResult _EstablishmentSummary(int id)
        {
            Establishment establishment = _establishmentRepository.Establishments.FirstOrDefault(p => p.EstablishmentId == id);
            if (establishment != null)
            {
                var parentCategories = establishment.Products.Select(item => item.Category.ParentCategory).Distinct().ToList();

                var model = new EstablishmentSummaryViewModel
                    {
                        Establishment = establishment,
                        ParentCategories = parentCategories
                    };
                return PartialView(model);
            }
            return null; // error state
        }
    }
}