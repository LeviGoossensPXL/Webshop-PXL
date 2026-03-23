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
            await _orderRepository.Delete(id);
        }
    }
}
