using Webshop.Application.Repositories;
using Webshop.Application.Results;
using Webshop.Domain.Entities;

namespace Webshop.Application.Services;

public class StockItemService : IStockItemService
{
    private readonly IStockItemRepository _stockItemRepository;
    private readonly IProductRepository _productRepository;

    public StockItemService(IStockItemRepository stockItemRepository, IProductRepository productRepository)
    {
        _stockItemRepository = stockItemRepository;
        _productRepository = productRepository;
    }

    public async Task<StockItemResult> AddAmountOfProduct(int productId, int amount)
    {
        var product = await _productRepository.GetById(productId);
        var stockItemResult = new StockItemResult();
        if (product == null)
        {
            stockItemResult.Failed("Product not found");
            return stockItemResult;
        }

        var stockItem = new StockItem
        {
            ProductId = productId,
            Quantity = amount
        };
        await _stockItemRepository.Add(stockItem);
        stockItemResult.StockItem = stockItem;
        return stockItemResult;
    }
}