using Webshop.Application.Repositories;
using Webshop.Application.Results;
using Webshop.Application.Services.Contracts;
using Webshop.Domain.Entities;

namespace Webshop.Application.Services;

public class StockItemService : IStockItemService
{
    private readonly IStockItemRepository _stockItemRepository;

    public StockItemService(IStockItemRepository stockItemRepository)
    {
        _stockItemRepository = stockItemRepository;
    }

    public Task<ServiceResultOfT<IEnumerable<StockItem>>> GetAll()
    {
        throw new NotImplementedException();
    }

    public Task<ServiceResultOfT<StockItem>> GetById(int id)
    {
        throw new NotImplementedException();
    }

    public Task<ServiceResult> Add(StockItem stockItem)
    {
        throw new NotImplementedException();
    }

    public Task<ServiceResult> Update(StockItem stockItem)
    {
        throw new NotImplementedException();
    }

    public Task<ServiceResult> Delete(int id)
    {
        throw new NotImplementedException();
    }
}