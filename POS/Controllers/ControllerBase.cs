namespace POS.Controllers
{
    #region

    using System;
    using System.Reflection;
    using System.Web.Mvc;

    using NLog;

    using POS.ActionFilters;

    #endregion

    [CustomValidateAntiForgeryToken(HttpVerbs.Post)]
    public class ControllerBase : Controller
    {
        #region Properties

        protected Logger Logger
        {
            get
            {
                Type declaringType = MethodBase.GetCurrentMethod().DeclaringType;
                if (declaringType != null)
                {
                    return LogManager.GetLogger(declaringType.ToString());
                }

                return null;
            }
        }

        #endregion
    }
}