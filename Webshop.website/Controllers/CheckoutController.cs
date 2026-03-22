using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using Webshop.Application.Repositories;
using Webshop.Domain.Entities;
using Webshop.website.ViewModels;

namespace Webshop.website.Controllers
{
    public class CheckoutController : Controller
    {
        private readonly IOrderRepository _orderRepository;
        private const string CartSessionKey = "ShoppingCart";

        public CheckoutController(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }

        // GET: Checkout/Index (Show the address form)
        [HttpGet]
        public IActionResult Index()
        {
            var cart = GetCartFromSession();

            // If cart is empty, send back to shop
            if (!cart.Items.Any())
            {
                return RedirectToAction("Index", "Shop");
            }

            return View(new OrderCreateViewModel());
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

               
                await _orderRepository.Add(order);

                // Empty the shopping cart after successful order
                HttpContext.Session.Remove(CartSessionKey);

                
                return View("Success", order.OrderId);
            }

            
            return View("Index", model);
        }

        private CartViewModel GetCartFromSession()
        {
            var sessionData = HttpContext.Session.GetString(CartSessionKey);
            return string.IsNullOrEmpty(sessionData) ? new CartViewModel() : JsonSerializer.Deserialize<CartViewModel>(sessionData) ?? new CartViewModel();
        }
    }
}