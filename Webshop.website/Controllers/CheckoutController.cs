using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Security.Claims;
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
                // Serialize the model to a JSON string to store it safely in TempData.
                // This keeps the address in memory while the user is on the payment page.
                TempData["OrderAddress"] = System.Text.Json.JsonSerializer.Serialize(model);

                return RedirectToAction("Payment");
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
        public IActionResult Payment()
        {
            // Redirect back if the user tries to access this page without filling in the address first
            if (!TempData.ContainsKey("OrderAddress")) return RedirectToAction("Index");

            // Keep the data alive in TempData for the next POST request
            TempData.Keep("OrderAddress");
            return View();
        }

        // POST: Checkout/ProcessPayment
        [HttpPost]
        public async Task<IActionResult> ProcessPayment()
        {
            if (!TempData.ContainsKey("OrderAddress")) return RedirectToAction("Index");

            // Retrieve the JSON string from TempData and convert it back to our view model
            var modelJson = TempData["OrderAddress"].ToString();
            var model = System.Text.Json.JsonSerializer.Deserialize<OrderCreateViewModel>(modelJson);

            var address = new Address
            {
                Street = model.Street,
                HouseNumber = model.HouseNumber,
                City = model.City,
                ZipCode = model.PostalCode,
                Country = model.Country
            };

            // Use the unique ID (NameIdentifier) instead of the name/email to ensure consistency with My Orders filter
            string userId = User.FindFirstValue(System.Security.Claims.ClaimTypes.NameIdentifier) ?? "GuestCustomer";

            // Create the actual order in the database now that we are sure the payment process is complete
            int orderId = await _checkoutService.CreateOrderAndConfirmAsync(address, userId);

            _cartService.ClearCart();

            return RedirectToAction("Success", new { orderId = orderId });
        }

        // GET: Checkout/Success
        [HttpGet]
        public IActionResult Success(int orderId)
        {
            return View(orderId);
        }
    }
}