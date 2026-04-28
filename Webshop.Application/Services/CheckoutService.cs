using System;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Webshop.Application.Services.Contracts;
using Webshop.Domain.Entities;

namespace Webshop.Application.Services
{
    public class CheckoutService : ICheckoutService
    {
        private readonly IOrderService _orderService;
        private readonly ICartService _cartService;
        private readonly IHttpClientFactory _httpClientFactory;

        public CheckoutService(IOrderService orderService, ICartService cartService, IHttpClientFactory httpClientFactory)
        {
            _orderService = orderService;
            _cartService = cartService;
            _httpClientFactory = httpClientFactory;
        }

        public async Task<int> CreateOrderAndConfirmAsync(Address deliveryAddress, string userId)
        {
            var cart = _cartService.GetCart();

            // Stop the process if the cart is empty to prevent blank orders
            if (cart == null || !cart.Items.Any())
            {
                return 0;
            }

            // Create the order directly with status Processing since payment is already done
            var order = new Order
            {
                UserId = userId,
                OrderDate = DateTime.UtcNow,
                Status = OrderStatus.Processing,
                DeliveryAddress = deliveryAddress,
                OrderLines = cart.Items.Select(item => new OrderLine
                {
                    ProductId = item.ProductId,
                    Quantity = item.Quantity,
                    UnitPrice = item.UnitPrice
                }).ToList()
            };

            await _orderService.Add(order);

            // Reduce stock in the external API for each item in the order
            try
            {
                var client = _httpClientFactory.CreateClient("StockApi");
                foreach (var line in order.OrderLines)
                {
                    await client.PutAsJsonAsync($"/StockItem/product/{line.ProductId}/reduce", line.Quantity);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Stock API reduce failed: {ex.Message}");
            }

            return order.OrderId;
        }
    }
}

