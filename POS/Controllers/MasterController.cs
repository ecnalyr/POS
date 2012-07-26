using System.Web.Mvc;
using POS.Domain.Abstract;

namespace POS.Controllers
{
    public class MasterController : Controller
    {
        #region Fields

        public IProductRepository repository;

        #endregion

        #region Constructors and Destructors

        public MasterController(IProductRepository productRepository)
        {
            repository = productRepository;
        }

        #endregion
    }
}
