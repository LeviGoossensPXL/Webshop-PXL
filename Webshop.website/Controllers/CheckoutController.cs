using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using Webshop.Application.Services.Contracts;
using Webshop.Domain.Entities;
using Webshop.website.ViewModels;

namespace Webshop.website.Controllers
{
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
                    item.ProductName = product?.Name ?? "Unknown Product";
                }
            }
            SaveCartToSession(cart);

            ViewBag.Cart = cart;
            ViewBag.UserName = "GuestCustomer";

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

        // POST: Checkout/Process (Save the order to database)
        [HttpPost]
        public async Task<IActionResult> Process(OrderCreateViewModel model)
        {
            var cart = GetCartFromSession();
            if (!cart.Items.Any()) return RedirectToAction("Index", "Shop");

            if (ModelState.IsValid)
            {
                // Create the real Order with products inside!
                var order = new Order
                {
                    UserId = "GuestCustomer", // When we add login later, we change this to real UserId
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

                // Empty the shopping cart after successful order
                HttpContext.Session.Remove(CartSessionKey);

                
                return View("Success", order.OrderId);
            }
            ViewBag.Cart = cart;
            ViewBag.UserName = "GuestCustomer";
            
            return View("Index", model);
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