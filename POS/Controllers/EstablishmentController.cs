using System.Linq;
using System.Web;
using System.Web.Mvc;
using POS.Domain.Abstract;
using POS.Domain.Model;

namespace POS.Controllers
{
    public class EstablishmentController : ControllerBase
    {
        #region Fields

        private readonly IEstablishmentRepository _establishmentRepository;

        #endregion

        #region Constructors and Destructors

        public EstablishmentController(IEstablishmentRepository establishmentRepo)
        {
            _establishmentRepository = establishmentRepo;
        }

        #endregion

        public ViewResult Index()
        {
            return View(_establishmentRepository.Establishments);
        }

        public ViewResult Edit(int id)
        {
            Establishment establishment =
                _establishmentRepository.Establishments.FirstOrDefault(p => p.EstablishmentId == id);
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

                _establishmentRepository.SaveEstablishment(establishment);
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
            Establishment establishment = _establishmentRepository.Establishments.FirstOrDefault(p => p.EstablishmentId == id);
            if (establishment != null)
            {
                _establishmentRepository.DeleteEstablishment(establishment);
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
