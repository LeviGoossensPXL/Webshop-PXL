namespace Webshop.website.ViewModels;

public class StockListViewModel
{
    public int StockItemId { get; set; }
    public int ProductId { get; set; }
    public string ProductName { get; set; } = string.Empty;
    public string? Sku { get; set; }
    public string Category { get; set; } = string.Empty;
    public int Quantity { get; set; }
    public string? WarehouseLocation { get; set; }
}
