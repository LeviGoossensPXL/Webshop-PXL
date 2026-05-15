using System.ComponentModel.DataAnnotations;

namespace Webshop.website.ViewModels;

public class StockEditViewModel
{
    public int StockItemId { get; set; }
    public int ProductId { get; set; }
    public string ProductName { get; set; } = string.Empty;

    [Required]
    public string? Sku { get; set; }

    [Required]
    [Range(0, 100000, ErrorMessage = "Quantity must be 0 or greater.")]
    public int Quantity { get; set; }

    [Required]
    public string? WarehouseLocation { get; set; }
}
