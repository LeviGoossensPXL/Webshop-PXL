using System;

namespace Webshop.MVC.Models
{
    // Dit model toont de samenvatting van een bestelling aan de klant.
    public class OrderConfirmViewModel
    {
        public int OrderId { get; set; }
        public DateTime Datum { get; set; }
        public decimal Totaal { get; set; }
        public string KlantNaam { get; set; } = string.Empty;
    }
}
