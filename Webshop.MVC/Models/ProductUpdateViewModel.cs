using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace Webshop.MVC.Models
{
    public class ProductUpdateViewModel
    {
        // Het ID van het product dat we bewerken
        public int Id { get; set; }

        [Required(ErrorMessage = "Naam is verplicht.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Beschrijving is verplicht.")]
        public string Description { get; set; }

        [Required(ErrorMessage = "Prijs is verplicht.")]
        [Range(0.01, 10000, ErrorMessage = "De prijs moet groter zijn dan 0.")]
        public decimal Price { get; set; }

        [Required(ErrorMessage = "SKU is verplicht.")]
        public string Sku { get; set; }

        // Huidige afbeelding URL (om te laten zien wat er nu is)
        public string? CurrentImageUrl { get; set; }

        // Nieuwe afbeelding uploaden (optioneel)
        public IFormFile? NewImage { get; set; }

        // Geselecteerde categorie ID (Foreign Key)
        [Required(ErrorMessage = "Selecteer een categorie.")]
        public int CategoryId { get; set; }

        // Lijst voor de dropdown (wordt gevuld in de controller)
        public IEnumerable<SelectListItem>? Categories { get; set; }
    }
}