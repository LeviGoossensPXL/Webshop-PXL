using System;

namespace Webshop.MVC.ViewModels
{
    // This model is fully compatible with the Product entity.
    public class ProductViewModel
    {
        public int Id { get; set; }

        
        public string Name { get; set; } = string.Empty;

        public decimal Price { get; set; } 
        public string Description { get; set; } = string.Empty;
        public string Sku { get; set; } = string.Empty;

        public string CategoryName { get; set; } = string.Empty;

        // For the images we will upload later.
        public string ImageUrl { get; set; } = "/images/default.jpg";
    }
}
