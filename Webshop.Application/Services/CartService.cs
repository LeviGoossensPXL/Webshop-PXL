using Microsoft.AspNetCore.Http;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Webshop.Application.Services.Contracts;
using Webshop.Domain.Entities;

namespace Webshop.Application.Services
{
    public class CartService : ICartService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IProductService _productService;
        private const string CartSessionKey = "ShoppingCart";

        public CartService(IHttpContextAccessor httpContextAccessor, IProductService productService)
        {
            _httpContextAccessor = httpContextAccessor;
            _productService = productService;
        }

        public ShoppingCart GetCart()
        {
            var sessionData = _httpContextAccessor.HttpContext?.Session.GetString(CartSessionKey);

            if (string.IsNullOrEmpty(sessionData))
            {
                return new ShoppingCart();
            }

            return JsonSerializer.Deserialize<ShoppingCart>(sessionData) ?? new ShoppingCart();
        }

        public async Task AddToCartAsync(int productId)
        {
            // Use ProductService to get the camping product
            var result = await _productService.GetById(productId);

            // If product is not found, stop here
            if (!result.Succeeded || result.Data == null)
            {
                return;
            }

            var product = result.Data;
            var cart = GetCart();

            // Check if this item is already in the cart
            var existingItem = cart.Items.FirstOrDefault(i => i.ProductId == productId);

            if (existingItem != null)
            {
                // Increase quantity
                existingItem.Quantity++;
            }
            else
            {
                // Add new item to cart
                cart.Items.Add(new ShoppingCartItem
                {
                    ProductId = product.ProductId,
                    ProductName = product.Name,
                    UnitPrice = product.Price,
                    Quantity = 1,
                    ImageUrl = product.ImageUrl
                });
            }

            SaveCart(cart);
        }

        public void RemoveFromCart(int productId)
        {
            var cart = GetCart();
            var item = cart.Items.FirstOrDefault(i => i.ProductId == productId);

            if (item != null)
            {
                cart.Items.Remove(item);
                SaveCart(cart);
            }
        }

        private void SaveCart(ShoppingCart cart)
        {
            var sessionData = JsonSerializer.Serialize(cart);
            _httpContextAccessor.HttpContext?.Session.SetString(CartSessionKey, sessionData);
        }

        public void ClearCart()
        {
            _httpContextAccessor.HttpContext?.Session.Remove(CartSessionKey);
        }

        public void UpdateQuantity(int productId, int change)
        {
            var cart = GetCart();
            var item = cart.Items.FirstOrDefault(i => i.ProductId == productId);

            if (item != null)
            {
                item.Quantity += change;
                if (item.Quantity <= 0)
                {
                    cart.Items.Remove(item);
                }
                SaveCart(cart);
            }
        }
    }
}