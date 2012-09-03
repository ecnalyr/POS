﻿namespace POS.Controllers
{
    #region

    using System.Linq;
    using System.Web.Mvc;

    using POS.Domain.Abstract;
    using POS.Domain.ApplicationService;
    using POS.Domain.Model;
    using POS.Domain.Properties;
    using POS.Models;

    #endregion

    public class CartController : Controller
    {
        #region Fields

        //private readonly CartApplicationService _cartApplicationService;

        private readonly ICartApplicationService _cartApplicationService;

        private readonly IProductRepository _repository;

        #endregion

        #region Constructors and Destructors

        public CartController(IProductRepository productRepository, /*CartApplicationService cartApplicationService*/ ICartApplicationService cartApplicationService)
        {
            this._repository = productRepository;
            this._cartApplicationService = cartApplicationService;
        }

        #endregion

        #region Public Methods and Operators

        public RedirectToRouteResult AddToCart(Cart cart, int productId, string returnUrl)
        {
            Product product = this._repository.Products.FirstOrDefault(p => p.ProductId == productId);

            if (product != null)
            {
                cart.AddItem(product, 1);
            }

            return RedirectToAction("Index", new { returnUrl });
        }

        public ViewResult Checkout()
        {
            return View(new ShippingDetails());
        }

        [HttpPost]
        public ViewResult Checkout(Cart cart, ShippingDetails shippingDetails)
        {
            if (!cart.Lines.Any())
            {
                ModelState.AddModelError(string.Empty, Resources.EmptyCartError);
            }

            if (ModelState.IsValid)
            {
                _cartApplicationService.Process(cart, shippingDetails);
                return View("Completed");
            }

            return View(shippingDetails);
        }

        public ViewResult Index(Cart cart, string returnUrl)
        {
            return View(new CartIndexViewModel { Cart = cart, ReturnUrl = returnUrl });
        }

        public RedirectToRouteResult RemoveFromCart(Cart cart, int productId, string returnUrl)
        {
            Product product = this._repository.Products.FirstOrDefault(p => p.ProductId == productId);

            if (product != null)
            {
                cart.RemoveLine(product);
            }

            return RedirectToAction("Index", new { returnUrl });
        }

        public ViewResult Summary(Cart cart)
        {
            return View(cart);
        }

        #endregion
    }
}