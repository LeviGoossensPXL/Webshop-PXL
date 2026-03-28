using Webshop.Domain.Entities;

namespace Webshop.Application.Services.Contracts
{
    public interface IOrderService
    {
        Task<IEnumerable<Order>> GetAll();
        Task<Order?> GetById(int id);
        Task Add(Order order);
        Task Update(Order order);
        Task Delete(int id);
        decimal CalculateTotalAmount(Order order);
        string GetFormattedDeliveryAddress(Order order);
        Task UpdateOrderStatus(int orderId, int newStatus);
    }
}