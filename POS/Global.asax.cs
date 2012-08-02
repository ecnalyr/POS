using System;
using System.Data.Entity;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using NLog;
using POS.Binders;
using POS.Infrastructure;
using POS.Domain.Model;

namespace POS
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : System.Web.HttpApplication
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }

        protected void Application_Error()
        {
            var ex = Context.Server.GetLastError();

            var logger = LogManager.GetLogger(GetType().ToString());

            if (ex is HttpException && ((HttpException)ex).GetHttpCode() == 404)
            {
                if (logger.IsWarnEnabled)
                {
                    var message =
                        String.Format(
                            "Unhandled exception trying to access " + Context.Request.Url +
                            ": {0} | Stack Trace: {1}",
                            ex.Message,
                            ex.StackTrace);

                    logger.Warn(message, ex);
                }
            }
            else
            {
                if (logger.IsFatalEnabled)
                {
                    var message =
                        String.Format(
                            "Unhandled exception trying to access " + Context.Request.Url +
                            ": {0} | Stack Trace: {1}",
                            ex.Message,
                            ex.StackTrace);

                    logger.Fatal(message, ex);
                }
            }
        }

        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                "Default", // Route name
                "{controller}/{action}/{id}", // URL with parameters
                new { controller = "Home", action = "Index", id = UrlParameter.Optional } // Parameter defaults
            );

        }

        protected void Application_Start()
        {
            Database.SetInitializer<EfDbContext>(new EfDbContextInitializer()); // Resets database

            AreaRegistration.RegisterAllAreas();

            RegisterGlobalFilters(GlobalFilters.Filters);
            RegisterRoutes(RouteTable.Routes);

            ModelBinders.Binders.Add(typeof(Cart), new CartModelBinder());
        }
    }
}