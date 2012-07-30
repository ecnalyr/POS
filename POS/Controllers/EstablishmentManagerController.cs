using System.Linq;
using System.Web;
using System.Web.Mvc;
using POS.Domain.Abstract;
using POS.Domain.Entities;

namespace POS.Controllers
{
    public class EstablishmentManagerController : ControllerBase
    {
        #region Constructors and Destructors

        public EstablishmentManagerController(IEstablishmentRepository establishmentRepository) : base(establishmentRepository)
        {
        }

        #endregion

        public ViewResult Index()
        {
            return View(EstablishmentRepository.Establishments);
        }

        public ViewResult Edit(int id)
        {
            Establishment establishment =
                EstablishmentRepository.Establishments.FirstOrDefault(p => p.EstablishmentId == id);
            return View(establishment);
        }

        [HttpPost]
        public ActionResult Edit(Establishment establishment, HttpPostedFileBase image)
        {
            if (ModelState.IsValid)
            {
                if (image != null)
                {
                    establishment.ImageMimeType = image.ContentType;
                    establishment.ImageData = new byte[image.ContentLength];
                    image.InputStream.Read(establishment.ImageData, 0, image.ContentLength);
                }

                EstablishmentRepository.SaveEstablishment(establishment);
                TempData["message"] = string.Format("Establishment {0} has been saved", establishment.Name);
                return RedirectToAction("Index");
            }
            else
            {
                // there is something wrong with the data values
                return View(establishment);
            }
        }

        [HttpPost]
        public ActionResult Delete(int id)
        {
            Establishment establishment = EstablishmentRepository.Establishments.FirstOrDefault(p => p.EstablishmentId == id);
            if (establishment != null)
            {
                EstablishmentRepository.DeleteEstablishment(establishment);
                TempData["message"] = string.Format("Establishment {0} was deleted", establishment.Name);
            }

            return RedirectToAction("Index");
        }

        public ViewResult Create()
        {
            return View("Edit", new Establishment());
        }
    }
}
