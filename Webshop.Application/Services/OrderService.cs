using System;
using System.Collections.Generic;
using System.Linq;
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

        public OrderService(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
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
            // safely fetch the order and update its status within the service layer to prevent direct entity manipulation in controllers
            var order = await _orderRepository.GetById(orderId);
            if (order == null)
            {
                result.Failed("Order not found. Status update failed.");
                return result;
            }
            order.Status = (OrderStatus)newStatus;
            await _orderRepository.Update(order);

            return result;
        }
    }
}
