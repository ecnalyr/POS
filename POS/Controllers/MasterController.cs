using System.Web.Mvc;
using POS.Domain.Abstract;

namespace POS.Controllers
{
    public class MasterController : Controller
    {
        #region Fields

        public IProductRepository ProductRepository;

        public IEstablishmentRepository EstablishmentRepository;

        #endregion

        #region Constructors and Destructors

        public MasterController(IProductRepository productRepo)
        {
            ProductRepository = productRepo;
        }

        public MasterController(IEstablishmentRepository establishmentRepo)
        {
            EstablishmentRepository = establishmentRepo;
        }

        #endregion
    }
}
