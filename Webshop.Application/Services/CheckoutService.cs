using System;
using System.Linq;
using System.Threading.Tasks;
using Webshop.Application.Services.Contracts;
using Webshop.Domain.Entities;

namespace Webshop.Application.Services
{
    public class CheckoutService : ICheckoutService
    {
        private readonly IOrderService _orderService;
        private readonly ICartService _cartService;

        public CheckoutService(IOrderService orderService, ICartService cartService)
        {
            _orderService = orderService;
            _cartService = cartService;
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

            return order.OrderId;
        }
    }
}