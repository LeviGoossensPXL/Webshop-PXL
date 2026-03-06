using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Webshop.Domain.Entities;

namespace Webshop.Application.Repositories
{
    public interface IStockItemRepository
    {
        Task<IEnumerable<StockItem>> GetAll();
        Task<StockItem?> GetById(int id);
        Task Add(StockItem stockItem);
        Task Update(StockItem stockItem);
        Task Delete(int id);
    }
}
