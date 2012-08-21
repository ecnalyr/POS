using System;
using System.Diagnostics;
using System.Linq;
using System.Web.Mvc;
using POS.Domain.Abstract;
using POS.Domain.Model;
using POS.Models;

namespace POS.Controllers
{
    using Domain.Properties;

    public class CartController : Controller
    {
        #region Fields

        private readonly IProductRepository repository;
        private readonly IOrderProcessor orderRepository;

        #endregion

        #region Constructors and Destructors

        public CartController(IProductRepository productRepository, IOrderProcessor processor)
        {
            repository = productRepository;
            orderRepository = processor;
        }

        #endregion

        #region Public Methods and Operators

        public RedirectToRouteResult AddToCart(Cart cart, int productId, string returnUrl)
        {
            Product product = repository.Products
                .FirstOrDefault(p => p.ProductId == productId);

            if (product != null) cart.AddItem(product, 1);

            return RedirectToAction("Index", new { returnUrl });
        }

        public RedirectToRouteResult RemoveFromCart(Cart cart, int productId, string returnUrl)
        {
            Product product = repository.Products
                .FirstOrDefault(p => p.ProductId == productId);

            if (product != null) cart.RemoveLine(product);

            return RedirectToAction("Index", new {returnUrl});
        }

        public ViewResult Index(Cart cart, string returnUrl)
        {
            return View(new CartIndexViewModel
            {
                Cart = cart,
                ReturnUrl = returnUrl
            });
        }

        public ViewResult Summary(Cart cart)
        {
            return View(cart);
        }

        public ViewResult Checkout()
        {
            return View(new ShippingDetails());
        }

        [HttpPost]
        public ViewResult Checkout(Cart cart, ShippingDetails shippingDetails)
        {
            if (!cart.Lines.Any()) ModelState.AddModelError("", Resources.EmptyCartError);

            if (ModelState.IsValid)
            {
                //CartApplicationService.Get(id);
                //CartApplicationService.Process(cart); // (Maybe it returns a hydrated Order object)
                //OrderApplicationService.Create(Order order);
                // then OrderApplicationService.Create(Order) calls EfOrderRepository

                orderRepository.ProcessOrder(cart, shippingDetails);
                return View("Completed");
            }
            return View(shippingDetails);
        }

        #endregion
    }
}