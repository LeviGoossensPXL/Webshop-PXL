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

    public async Task<ServiceResultOfT<IEnumerable<StockItem>>> GetAll()
    {
        var result = new ServiceResultOfT<IEnumerable<StockItem>>();
        var stocks = await _stockItemRepository.GetAll();

        result.Data = stocks;
        return result;
    }

    public Task<ServiceResultOfT<StockItem>> GetById(int id)
    {
        throw new NotImplementedException();
    }

    public async Task<ServiceResultOfT<StockItem>> GetByProductId(int id)
    {
        var result = new ServiceResultOfT<StockItem>();
        var stockItems = await _stockItemRepository.GetAll();
        var stockItem = stockItems.FirstOrDefault(x => x.ProductId == id);

        if (stockItem == null)
        {
            result.Failed("StockItem not found.");
            return result;
        }

        result.Data = stockItem;
        return result;
    }

    public async Task<ServiceResult> Add(StockItem stockItem)
    {
        var result = new ServiceResult();

        var stockItems = await _stockItemRepository.GetAll();
        var existingStockItem = stockItems.FirstOrDefault(x => x.ProductId == stockItem.ProductId);

        if (existingStockItem != null)
        {
            result.Failed($"Stock already exists for ProductId {stockItem.ProductId}");
            return result;
        }

        await _stockItemRepository.Add(stockItem);
        return result;
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