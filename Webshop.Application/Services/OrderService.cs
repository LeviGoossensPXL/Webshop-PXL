using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using Webshop.Application.Repositories;
using Webshop.Application.Results;
using Webshop.Application.Services.Contracts;
using Webshop.Domain.Entities;

namespace Webshop.Application.Services
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IHttpClientFactory _httpClientFactory;

        public OrderService(IOrderRepository orderRepository, IHttpClientFactory httpClientFactory)
        {
            _orderRepository = orderRepository;
            _httpClientFactory = httpClientFactory;
        }

        public async Task<ServiceResultOfT<IEnumerable<Order>>> GetAll()
        {
            var result = new ServiceResultOfT<IEnumerable<Order>>();
            result.Data = await _orderRepository.GetAll();
            return result;
        }

        public async Task<ServiceResultOfT<Order>> GetById(int id)
        {
            var result = new ServiceResultOfT<Order>();
            var order = await _orderRepository.GetById(id);

            // return a failed result instead of null if the entity does not exist
            if (order == null)
            {
                result.Failed("Order not found.");
                return result;
            }

            result.Data = order;
            return result;
        }

        public async Task<ServiceResultOfT<IEnumerable<Order>>> GetOrdersByUserId(string userId)
        {
            var result = new ServiceResultOfT<IEnumerable<Order>>();
            result.Data = await _orderRepository.GetByUserId(userId);
            return result;
        }

        public async Task<ServiceResult> Add(Order order)
        {
            var result = new ServiceResult();
            await _orderRepository.Add(order);
            return result;
        }

        public async Task<ServiceResult> Update(Order order)
        {
            var result = new ServiceResult();
            await _orderRepository.Update(order);
            return result;
        }

        public async Task<ServiceResult> Delete(int id)
        {
            var result = new ServiceResult();
            // verify order doesn't exists before attempting deletion to prevent crashes
            var order = await _orderRepository.GetById(id);
            if (order == null)
            {
                result.Failed("Order not found and could not be deleted.");
                return result;
            }
            await _orderRepository.Delete(id);
            return result;
        }
        public decimal CalculateTotalAmount(Order order)
        {
            // multiply quantity by unit price for each order line and calculate the total sum
            return order.OrderLines?.Sum(ol => ol.Quantity * ol.UnitPrice) ?? 0;
        }

        public string GetFormattedDeliveryAddress(Order order)
        {
            // check if address exists to prevent null reference exceptions
            if (order.DeliveryAddress == null)
            {
                return "No address provided";
            }

            return $"{order.DeliveryAddress.Street} {order.DeliveryAddress.HouseNumber}, {order.DeliveryAddress.ZipCode} {order.DeliveryAddress.City}, {order.DeliveryAddress.Country}";
        }
        public async Task<ServiceResult> UpdateOrderStatus(int orderId, int newStatus)
        {
            var result = new ServiceResult();
            var order = await _orderRepository.GetById(orderId);
            if (order == null)
            {
                result.Failed("Order not found. Status update failed.");
                return result;
            }

            var oldStatus = order.Status;
            var newOrderStatus = (OrderStatus)newStatus;

            // Do not take any action if the order status has not changed
            if (oldStatus == newOrderStatus) return result;

          
            // If the order is cancelled: Restore stock
            if (newOrderStatus == OrderStatus.Cancelled && oldStatus != OrderStatus.Cancelled)
            {
                var stockResult = await ManageStockUpdate(order, "add");
                if (!stockResult.Succeeded) return stockResult;
            }
            // If the order is being reinstated after cancellation (Pending/Processing, etc.): Reduce stock again
            else if (oldStatus == OrderStatus.Cancelled && newOrderStatus != OrderStatus.Cancelled)
            {
                var stockResult = await ManageStockUpdate(order, "reduce");
                if (!stockResult.Succeeded) return stockResult;
            }

            // If stock operations are successful, update the order status
            order.Status = newOrderStatus;
            await _orderRepository.Update(order);

            return result;
        }

        private async Task<ServiceResult> ManageStockUpdate(Order order, string action)
        {
            var result = new ServiceResult();
            if (order.OrderLines == null || !order.OrderLines.Any())
            {
                return result;
            }

            try
            {
                var client = _httpClientFactory.CreateClient("StockApi");
                foreach (var line in order.OrderLines)
                {
                    var response = await client.PutAsJsonAsync($"/StockItem/product/{line.ProductId}/{action}", line.Quantity);
                    if (!response.IsSuccessStatusCode)
                    {
                        var error = await response.Content.ReadAsStringAsync();
                        result.Failed($"Stock API {action} failed for product {line.ProductId}: {error}");
                        return result;
                    }
                }
            }
            catch (Exception ex)
            {
                result.Failed($"Stock API connection error during {action}: {ex.Message}");
                return result;
            }

            return result;
        }
    }
}

