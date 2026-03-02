using System;

namespace Webshop.MVC.Models
{
    // Dit model is volledig compatibel met de Product entiteit.
    public class ProductViewModel
    {
        public int Id { get; set; }

        
        public string Naam { get; set; } = string.Empty;

       
        public string PrijsDisplay { get; set; } = string.Empty;

       
        public string CategorieNaam { get; set; } = string.Empty;

        // Voor de afbeeldingen die we later gaan uploaden.
        public string FotoUrl { get; set; } = "/images/default.jpg";
    }
}
