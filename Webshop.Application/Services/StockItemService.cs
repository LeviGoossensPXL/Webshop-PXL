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
}