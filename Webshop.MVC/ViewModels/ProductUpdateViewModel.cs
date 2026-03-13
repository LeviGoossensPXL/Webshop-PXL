using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace Webshop.MVC.ViewModels
{
    public class ProductUpdateViewModel
    {
        // The ID of the product we are editing
        public int Id { get; set; }

        [Required(ErrorMessage = "Name is required.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Description is required.")]
        public string Description { get; set; }

        [Required(ErrorMessage = "Price is required.")]
        [Range(0.01, 10000, ErrorMessage = "The price must be greater than 0.")]
        public decimal Price { get; set; }

        [Required(ErrorMessage = "SKU is required.")]
        public string Sku { get; set; }

        // Current image URL (to show what is there now)
        public string? CurrentImageUrl { get; set; }

        // Upload new image (optional)
        public IFormFile? NewImage { get; set; }

        // Selected category ID (Foreign Key)
        [Required(ErrorMessage = "Please select a category.")]
        public int CategoryId { get; set; }

        // List for the dropdown (filled in the controller)
        public IEnumerable<SelectListItem>? Categories { get; set; }
    }
}