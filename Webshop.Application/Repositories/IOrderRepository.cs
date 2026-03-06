using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Webshop.Domain.Entities;

namespace Webshop.Application.Repositories
{
    public interface IOrderRepository
    {
        Task<IEnumerable<Order>> GetAll();
        Task<Order?> GetById(int id);
        Task Add(Order order);
        Task Update(Order order);
        Task Delete(int id);
    }
}
