using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using Webshop.Application.Repositories;
using Webshop.MVC.ViewModels;

namespace Webshop.MVC.Controllers
{
    public class CartController : Controller
    {
        private readonly IProductRepository _productRepository;

        // The key used to find the cart in the session memory
        private const string CartSessionKey = "ShoppingCart";

        public CartController(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        // GET: Cart/Index (Show the shopping cart page)
        [HttpGet]
        public IActionResult Index()
        {
            var cart = GetCartFromSession();
            return View(cart);
        }

        // POST: Cart/AddToCart/5 (Add an item to the cart)
        [HttpPost]
        public async Task<IActionResult> AddToCart(int productId)
        {
            var product = await _productRepository.GetById(productId);
            if (product == null)
            {
                return NotFound();
            }

            var cart = GetCartFromSession();

            // Check if this camping product is already in the cart
            var existingItem = cart.Items.FirstOrDefault(i => i.ProductId == productId);

            if (existingItem != null)
            {
                // Increase the quantity if it is already there
                existingItem.Quantity++;
            }
            else
            {
                // Add the new camping product to the cart
                cart.Items.Add(new CartItemViewModel
                {
                    ProductId = product.ProductId,
                    ProductName = product.Name,
                    UnitPrice = product.Price,
                    Quantity = 1,
                    ImageUrl = product.ImageUrl
                });
            }

            SaveCartToSession(cart);

            // Send the user back to the shop catalog
            return RedirectToAction("Index", "Shop");
        }

        // POST: Cart/RemoveFromCart/5 (Remove an item completely)
        [HttpPost]
        public IActionResult RemoveFromCart(int productId)
        {
            var cart = GetCartFromSession();
            var item = cart.Items.FirstOrDefault(i => i.ProductId == productId);

            if (item != null)
            {
                cart.Items.Remove(item);
                SaveCartToSession(cart);
            }

            // Reload the cart page
            return RedirectToAction("Index");
        }

        // --- Helper Methods for Session Management ---

        // Get the current cart from browser session
        private CartViewModel GetCartFromSession()
        {
            var sessionData = HttpContext.Session.GetString(CartSessionKey);
            if (string.IsNullOrEmpty(sessionData))
            {
                return new CartViewModel();
            }

            return JsonSerializer.Deserialize<CartViewModel>(sessionData) ?? new CartViewModel();
        }

        // Save the updated cart back to browser session
        private void SaveCartToSession(CartViewModel cart)
        {
            var sessionData = JsonSerializer.Serialize(cart);
            HttpContext.Session.SetString(CartSessionKey, sessionData);
        }
    }
}