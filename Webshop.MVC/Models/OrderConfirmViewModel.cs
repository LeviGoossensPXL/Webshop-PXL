using System;

namespace Webshop.MVC.Models
{
    // Dit model toont de samenvatting van een bestelling aan de klant.
    public class OrderConfirmViewModel
    {
        public int OrderId { get; set; }
        public DateTime Date { get; set; }
        public decimal Total { get; set; }
        public string CustomerName { get; set; } = string.Empty;
    }
}
