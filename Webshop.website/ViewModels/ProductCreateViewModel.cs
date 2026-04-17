using System.ComponentModel.DataAnnotations;
using Webshop.Domain.Entities;

namespace Webshop.website.ViewModels
{
    public class ProductCreateViewModel
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public decimal Price { get; set; }
        public string? ImageUrl { get; set; }
        [Required]
        public string Sku { get; set; }
        [Required]
        public int CategoryId { get; set; }

        // Stock fields — sent to the Stock Web API when creating a product
        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Initial stock must be at least 1.")]
        [Display(Name = "Initial Stock Quantity")]
        public int InitialStock { get; set; } = 1;

        [Display(Name = "Warehouse Location")]
        public string WarehouseLocation { get; set; } = "Default Warehouse";
    }
}

