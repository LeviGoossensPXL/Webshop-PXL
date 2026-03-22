using Webshop.Domain.Entities;

namespace Webshop.Application.Results;

public class StockItemResult : BaseResult
{
    public StockItem? StockItem { get; set; }
}