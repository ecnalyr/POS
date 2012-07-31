using System.Web.Mvc;
using POS.Domain.Model;

namespace POS.Binders
{
    public class CartModelBinder : IModelBinder
    {
        private const string SessionKey = "Cart";

        #region IModelBinder Members

        public object BindModel(ControllerContext controllerContext,
                                ModelBindingContext bindingContext)
        {
            // get the Cart from the session

            // ReSharper disable PossibleNullReferenceException
            var cart = (Cart) controllerContext.HttpContext.Session[SessionKey];
            // ReSharper restore PossibleNullReferenceException

            if (cart == null)
            {
                cart = new Cart();
                controllerContext.HttpContext.Session[SessionKey] = cart;
            }
            return cart;
        }

        #endregion
    }
}