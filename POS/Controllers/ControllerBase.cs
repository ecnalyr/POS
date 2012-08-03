using System;
using System.Reflection;
using System.Web.Mvc;
using NLog;
using POS.Domain.Abstract;
using POS.ActionFilters;

namespace POS.Controllers
{
    [CustomValidateAntiForgeryTokenAttribute(HttpVerbs.Post)]
    public class ControllerBase : Controller
    {
        #region Fields

        #endregion

        #region Constructors and Destructors


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