using System;
using System.Reflection;
using System.Web.Mvc;
using NLog;
using POS.Domain.Abstract;

namespace POS.Controllers
{
    public class ControllerBase : Controller
    {
        #region Fields

        public IEstablishmentRepository EstablishmentRepository;
        public IProductRepository ProductRepository;

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

        protected Logger Logger
        {
            get
            {
                Type declaringType = MethodBase.GetCurrentMethod().DeclaringType;
                if (declaringType != null)
                    return LogManager.GetLogger(declaringType.ToString());
                return null;
            }
        }
    }
}