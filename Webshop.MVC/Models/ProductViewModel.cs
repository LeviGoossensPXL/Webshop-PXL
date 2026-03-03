using System;

namespace Webshop.MVC.Models
{
    // Dit model is volledig compatibel met de Product entiteit.
    public class ProductViewModel
    {
        public int Id { get; set; }

        
        public string Name { get; set; } = string.Empty;

       
        public string PriceDisplay { get; set; } = string.Empty;

       
        public string CategoryName { get; set; } = string.Empty;

        // Voor de afbeeldingen die we later gaan uploaden.
        public string ImageUrl { get; set; } = "/images/default.jpg";
    }
}
