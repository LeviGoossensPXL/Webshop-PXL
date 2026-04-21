using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;
using Webshop.Application.Services.Contracts;
using Webshop.Domain.Entities;
using Webshop.website.ViewModels;

namespace Webshop.website.Controllers
{
    [Authorize]
    public class CheckoutController : Controller
    {
        private readonly ICheckoutService _checkoutService;
        private readonly ICartService _cartService;

        public CheckoutController(ICheckoutService checkoutService, ICartService cartService)
        {
            _checkoutService = checkoutService;
            _cartService = cartService;
        }

        // GET: Checkout/Index
        [HttpGet]
        public IActionResult Index()
        {
            var cart = _cartService.GetCart();

            if (!cart.Items.Any())
            {
                return RedirectToAction("Index", "Shop");
            }

            // Map Domain cart to ViewModel
            var cartVm = new CartViewModel
            {
                Items = cart.Items.Select(i => new CartItemViewModel
                {
                    ProductId = i.ProductId,
                    ProductName = i.ProductName,
                    UnitPrice = i.UnitPrice,
                    Quantity = i.Quantity,
                    ImageUrl = i.ImageUrl
                }).ToList()
            };

            ViewBag.Cart = cartVm;
            ViewBag.UserName = User.Identity?.Name ?? "GuestCustomer";

            return View(new OrderCreateViewModel());
        }

        [HttpPost]
        public IActionResult UpdateQuantity(int productId, int change)
        {
            _cartService.UpdateQuantity(productId, change);
            return RedirectToAction("Index");
        }

        // POST: Checkout/Process
        [HttpPost]
        public async Task<IActionResult> Process(OrderCreateViewModel model)
        {
            var cart = _cartService.GetCart();
            if (!cart.Items.Any()) return RedirectToAction("Index", "Shop");

            if (ModelState.IsValid)
            {
                // Map ViewModel to Domain Entity before sending to Application layer
                var address = new Address
                {
                    Street = model.Street,
                    HouseNumber = model.HouseNumber,
                    City = model.City,
                    ZipCode = model.PostalCode,
                    Country = model.Country
                };

                string userId = User.Identity?.Name ?? "GuestCustomer";

                // Process the order via the new CheckoutService
                int orderId = await _checkoutService.ProcessOrderAsync(address, userId);

                return RedirectToAction("Payment", new { orderId = orderId });
            }

            // If mapping fails, rebuild the cart view
            var cartVm = new CartViewModel
            {
                Items = cart.Items.Select(i => new CartItemViewModel
                {
                    ProductId = i.ProductId,
                    ProductName = i.ProductName,
                    UnitPrice = i.UnitPrice,
                    Quantity = i.Quantity,
                    ImageUrl = i.ImageUrl
                }).ToList()
            };

            ViewBag.Cart = cartVm;
            ViewBag.UserName = User.Identity?.Name ?? "GuestCustomer";
            return View("Index", model);
        }

        // GET: Checkout/Payment
        [HttpGet]
        public IActionResult Payment(int orderId)
        {
            ViewBag.OrderId = orderId;
            return View();
        }

        // POST: Checkout/ProcessPayment
        [HttpPost]
        public async Task<IActionResult> ProcessPayment(int orderId)
        {
            // Payment confirmed — now reduce stock and update order status
            await _checkoutService.ConfirmPaymentAsync(orderId);

            // Empty the shopping cart
            _cartService.ClearCart();

            return View("PaymentSuccess", orderId);
        }

        // GET: Checkout/Success
        [HttpGet]
        public IActionResult Success(int orderId)
        {
            return View(orderId);
        }
    }
}