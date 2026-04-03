using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;
using Webshop.Application.Services.Contracts;
using Webshop.website.ViewModels;

namespace Webshop.website.Controllers
{
    public class CartController : Controller
    {
        private readonly ICartService _cartService;

        public CartController(ICartService cartService)
        {
            _cartService = cartService;
        }

        // GET: Cart/Index
        [HttpGet]
        public IActionResult Index()
        {
            // Get the cart from Application layer
            var cart = _cartService.GetCart();

            // Map the Domain entity to ViewModel
            var viewModel = new CartViewModel
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

            return View(viewModel);
        }

        // POST: Cart/AddToCart/5 
        [HttpPost]
        public async Task<IActionResult> AddToCart(int productId)
        {
            await _cartService.AddToCartAsync(productId);
            return RedirectToAction("Index", "Shop");
        }

        // POST: Cart/RemoveFromCart/5 
        [HttpPost]
        public IActionResult RemoveFromCart(int productId)
        {
            _cartService.RemoveFromCart(productId);
            return RedirectToAction("Index");
        }
    }
}