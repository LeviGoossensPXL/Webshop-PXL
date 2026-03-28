using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Webshop.Application.Repositories;
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

        public async Task<IEnumerable<Order>> GetAll()
        {
            return await _orderRepository.GetAll();
        }

        public async Task<Order?> GetById(int id)
        {
            return await _orderRepository.GetById(id);
        }

        public async Task Add(Order order)
        {
            await _orderRepository.Add(order);
        }

        public async Task Update(Order order)
        {
            await _orderRepository.Update(order);
        }

        public async Task Delete(int id)
        {
            // verify order exists before attempting deletion to prevent crashes
            var order = await _orderRepository.GetById(id);
            if (order != null)
            {
                await _orderRepository.Delete(id);
            }
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
        public async Task UpdateOrderStatus(int orderId, int newStatus)
        {
            // safely fetch the order and update its status within the service layer to prevent direct entity manipulation in controllers
            var order = await _orderRepository.GetById(orderId);
            if (order != null)
            {
                order.Status = (OrderStatus)newStatus;
                await _orderRepository.Update(order);
            }
        }
    }
}
