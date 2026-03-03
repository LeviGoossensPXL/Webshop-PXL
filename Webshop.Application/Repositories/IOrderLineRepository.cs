using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Webshop.Domain.Entities;

namespace Webshop.Application.Repositories
{
    public interface IOrderLineRepository
    {
        Task<IEnumerable<OrderLine>> GetAll();
        Task<OrderLine?> GetById(int id);
        Task Add(OrderLine orderLine);
        Task Update(OrderLine orderLine);
        Task Delete(int id);
    }
}
