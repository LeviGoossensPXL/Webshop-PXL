using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using Webshop.Application.Services.Contracts;
using Webshop.Domain.Entities;
using Webshop.website.ViewModels;

namespace Webshop.website.Controllers
{
    [Authorize]
    public class CheckoutController : Controller
    {
        private readonly IOrderService _orderService;
        private readonly IProductService _productService;
        private const string CartSessionKey = "ShoppingCart";

        public CheckoutController(IOrderService orderService, IProductService productService)
        {
            _orderService = orderService;
            _productService = productService;
        }

        // GET: Checkout/Index (Show the address form)
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var cart = GetCartFromSession();

            // If cart is empty, send back to shop
            if (!cart.Items.Any())
            {
                return RedirectToAction("Index", "Shop");
            }

            foreach (var item in cart.Items)
            {
                if (string.IsNullOrEmpty(item.ProductName))
                {
                    var product = await _productService.GetById(item.ProductId);
                    item.ProductName = product.Data?.Name ?? "Unknown Product";
                }
            }
            SaveCartToSession(cart);

            ViewBag.Cart = cart;
            // Use the logged-in user's email, or fallback to GuestCustomer
            ViewBag.UserName = User.Identity?.Name ?? "GuestCustomer";

            return View(new OrderCreateViewModel());
        }

        [HttpPost]
        public IActionResult UpdateQuantity(int productId, int change)
        {
            var cart = GetCartFromSession();
            var item = cart.Items.FirstOrDefault(i => i.ProductId == productId);

            if (item != null)
            {
                item.Quantity += change;
                if (item.Quantity <= 0)
                {
                    cart.Items.Remove(item);
                }
                SaveCartToSession(cart);
            }

            return RedirectToAction("Index");
        }

        // POST: Checkout/Process (Save the order to database and go to payment)
        [HttpPost]
        public async Task<IActionResult> Process(OrderCreateViewModel model)
        {
            var cart = GetCartFromSession();
            if (!cart.Items.Any()) return RedirectToAction("Index", "Shop");

            if (ModelState.IsValid)
            {
                // Create the real Order with status Pending
                var order = new Order
                {
                    UserId = User.Identity?.Name ?? "GuestCustomer",
                    OrderDate = DateTime.UtcNow,
                    Status = OrderStatus.Pending,
                    DeliveryAddress = new Address
                    {
                        Street = model.Street,
                        HouseNumber = model.HouseNumber,
                        City = model.City,
                        ZipCode = model.PostalCode,
                        Country = model.Country
                    },
                    // Convert Cart Items to OrderLines
                    OrderLines = cart.Items.Select(item => new OrderLine
                    {
                        ProductId = item.ProductId,
                        Quantity = item.Quantity,
                        UnitPrice = item.UnitPrice
                    }).ToList()
                };

                await _orderService.Add(order);

                // Redirect to the new Payment page with the generated Order ID
                return RedirectToAction("Payment", new { orderId = order.OrderId });
            }

            ViewBag.Cart = cart;
            ViewBag.UserName = User.Identity?.Name ?? "GuestCustomer";
            return View("Index", model);
        }

        // GET: Checkout/Payment (Show fake payment page)
        [HttpGet]
        public IActionResult Payment(int orderId)
        {
            ViewBag.OrderId = orderId;
            return View();
        }

        // POST: Checkout/ProcessPayment (Complete the fake payment)
        [HttpPost]
        public IActionResult ProcessPayment(int orderId)
        {
            // Empty the shopping cart ONLY after successful payment
            HttpContext.Session.Remove(CartSessionKey);

            // Show the intermediate success and redirect page instead of jumping directly
            return View("PaymentSuccess", orderId);
        }

        // GET: Checkout/Success (Show order complete page)
        [HttpGet]
        public IActionResult Success(int orderId)
        {
            return View(orderId);
        }

        private CartViewModel GetCartFromSession()
        {
            var sessionData = HttpContext.Session.GetString(CartSessionKey);
            return string.IsNullOrEmpty(sessionData) ? new CartViewModel() : JsonSerializer.Deserialize<CartViewModel>(sessionData) ?? new CartViewModel();
        }

        private void SaveCartToSession(CartViewModel cart)
        {
            HttpContext.Session.SetString(CartSessionKey, JsonSerializer.Serialize(cart));
        }
    }
}