using Webshop.Domain.Entities;

namespace Webshop.MVC.Models
{
    
    public class OrderViewModel
    {
        public int OrderId { get; set; }
        public string KlantNaam { get; set; } = string.Empty;
        // De status als tekst (bijv. "In behandeling").
        public string StatusBeschrijving { get; set; } = string.Empty;

        public Adres DeliveryAddress { get; set; } = new Adres();

        public decimal TotaalBedrag { get; set; }
    }
}
