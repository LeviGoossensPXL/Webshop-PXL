namespace WebApi.Dtos;

public class CreateStockItemDto
{
    public string? Sku { get; set; }
    public int Quantity { get; set; }
    public string? WarehouseLocation { get; set; }

    // Logical link to Product in the Webshop database (no FK, separate DB)
    public int ProductId { get; set; }
}