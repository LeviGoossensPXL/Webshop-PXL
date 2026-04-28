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

    public async Task<ServiceResultOfT<StockItem>> GetById(int id)
    {
        var result = new ServiceResultOfT<StockItem>();
        var stockItem = await _stockItemRepository.GetById(id);

        if (stockItem == null)
        {
            result.Failed("StockItem not found.");
            return result;
        }

        result.Data = stockItem;
        return result;
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

    public async Task<ServiceResult> Update(StockItem stockItem)
    {
        var result = new ServiceResult();
        var existing = await _stockItemRepository.GetById(stockItem.StockItemId);

        if (existing == null)
        {
            result.Failed($"StockItem with id {stockItem.StockItemId} not found.");
            return result;
        }

        await _stockItemRepository.Update(stockItem);
        return result;
    }

    public async Task<ServiceResult> DeleteByProductId(int productId)
    {
        var result = new ServiceResult();

        var stockItems = await _stockItemRepository.GetAll();
        var existingStockItem = stockItems.FirstOrDefault(x => x.ProductId == productId);

        if (existingStockItem == null)
        {
            result.Failed($"No stock found for ProductId {productId}");
            return result;
        }

        await _stockItemRepository.Delete(existingStockItem.StockItemId);
        return result;
    }
}