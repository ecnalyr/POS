using System.Web.Mvc;
using POS.Domain.Abstract;

namespace POS.Controllers
{
    public class ControllerBase : Controller
    {
        #region Fields

        public IProductRepository ProductRepository;

        public IEstablishmentRepository EstablishmentRepository;

        #endregion

        #region Constructors and Destructors

        public ControllerBase(IProductRepository productRepo)
        {
            ProductRepository = productRepo;
        }

        public ControllerBase(IEstablishmentRepository establishmentRepo)
        {
            EstablishmentRepository = establishmentRepo;
        }

        #endregion
    }
}
