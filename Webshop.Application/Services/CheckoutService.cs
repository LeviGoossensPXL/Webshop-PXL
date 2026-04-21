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

        public async Task<int> ProcessOrderAsync(Address deliveryAddress, string userId)
        {
            var cart = _cartService.GetCart();

            if (cart == null || !cart.Items.Any())
            {
                return 0; // Prevent empty orders
            }

            var order = new Order
            {
                UserId = userId,
                OrderDate = DateTime.UtcNow,
                Status = OrderStatus.Pending,
                DeliveryAddress = deliveryAddress,
                OrderLines = cart.Items.Select(item => new OrderLine
                {
                    ProductId = item.ProductId,
                    Quantity = item.Quantity,
                    UnitPrice = item.UnitPrice
                }).ToList()
            };

            await _orderService.Add(order);

            // Stock is NOT reduced here — it will be reduced after payment confirmation
            return order.OrderId;
        }

        public async Task ConfirmPaymentAsync(int orderId)
        {
            // Fetch the order with its lines
            var result = await _orderService.GetById(orderId);
            if (!result.Succeeded || result.Data == null) return;

            var order = result.Data;

            // Update order status to Processing (payment confirmed)
            await _orderService.UpdateOrderStatus(orderId, (int)OrderStatus.Processing);

            // Now reduce stock in the Stock Web API
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
        }
    }
}
