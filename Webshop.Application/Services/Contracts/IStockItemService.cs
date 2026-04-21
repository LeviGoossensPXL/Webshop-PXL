using Webshop.Application.Repositories;
using Webshop.Application.Results;
using Webshop.Application.Services.Contracts;
using Webshop.Domain.Entities;

namespace Webshop.Application.Services.Contracts;

public interface IStockItemService
{
    Task<ServiceResultOfT<IEnumerable<StockItem>>> GetAll();
    Task<ServiceResultOfT<StockItem>> GetById(int id);
    Task<ServiceResultOfT<StockItem>> GetByProductId(int id);
    Task<ServiceResult> Add(StockItem stockItem);
    Task<ServiceResult> Update(StockItem stockItem);
    Task<ServiceResult> Delete(int id);
}