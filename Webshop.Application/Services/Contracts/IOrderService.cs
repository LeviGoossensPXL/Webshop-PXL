using Webshop.Application.Results;
using Webshop.Domain.Entities;

namespace Webshop.Application.Services.Contracts
{
    public interface IOrderService
    {
        Task<ServiceResultOfT<IEnumerable<Order>>> GetAll();
        Task<ServiceResultOfT<Order>> GetById(int id);
        Task<ServiceResultOfT<IEnumerable<Order>>> GetOrdersByUserId(string userId);
        Task<ServiceResult> Add(Order order);
        Task<ServiceResult> Update(Order order);
        Task<ServiceResult> Delete(int id);
        decimal CalculateTotalAmount(Order order);
        string GetFormattedDeliveryAddress(Order order);
        Task<ServiceResult> UpdateOrderStatus(int orderId, int newStatus);
    }
}