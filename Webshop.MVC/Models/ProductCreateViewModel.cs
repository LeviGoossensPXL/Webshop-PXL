using System.ComponentModel.DataAnnotations;
using Webshop.Domain.Entities;

namespace Webshop.MVC.Models
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
    }
}
