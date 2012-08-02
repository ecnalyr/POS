using System;
using System.Reflection;
using System.Web.Mvc;
using NLog;
using POS.Domain.Abstract;

namespace POS.Controllers
{
    [UseAntiForgeryTokenOnPostByDefault]
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


        [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
        public class BypassAntiForgeryTokenAttribute : ActionFilterAttribute
        {
        }

        [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
        public class UseAntiForgeryTokenOnPostByDefault : ActionFilterAttribute
        {
            public override void OnActionExecuting(ActionExecutingContext filterContext)
            {
                if (ShouldValidateAntiForgeryTokenManually(filterContext))
                {
                    var authorizationContext = new AuthorizationContext(filterContext.Controller.ControllerContext);

                    //Use the authorization of the anti forgery token, 
                    //which can't be inhereted from because it is sealed
                    new ValidateAntiForgeryTokenAttribute().OnAuthorization(authorizationContext);
                }

                base.OnActionExecuting(filterContext);
            }

            /// <summary>
            /// We should validate the anti forgery token manually if the following criteria are met:
            /// 1. The http method must be POST
            /// 2. There is not an existing [ValidateAntiForgeryToken] attribute on the action
            /// 3. There is no [BypassAntiForgeryToken] attribute on the action
            /// </summary>
            private static bool ShouldValidateAntiForgeryTokenManually(ActionExecutingContext filterContext)
            {
                var httpMethod = filterContext.HttpContext.Request.HttpMethod;

                //1. The http method must be POST
                if (httpMethod != "POST") return false;

                // 2. There is not an existing anti forgery token attribute on the action
                var antiForgeryAttributes =
                    filterContext.ActionDescriptor.GetCustomAttributes(typeof (ValidateAntiForgeryTokenAttribute), false);

                if (antiForgeryAttributes.Length > 0) return false;

                // 3. There is no [BypassAntiForgeryToken] attribute on the action
                var ignoreAntiForgeryAttributes =
                    filterContext.ActionDescriptor.GetCustomAttributes(typeof (BypassAntiForgeryTokenAttribute), false);

                if (ignoreAntiForgeryAttributes.Length > 0) return false;

                return true;
            }
        }
    }
}