using Webshop.Application.Results;

namespace Webshop.Application.Services;

public interface IStockItemService
{
    public Task<StockItemResult> AddAmountOfProduct(int productId, int amount);
}