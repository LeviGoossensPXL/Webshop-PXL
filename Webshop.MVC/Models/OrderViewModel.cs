using Webshop.Domain.Entities;

namespace Webshop.MVC.Models
{
    
    public class OrderViewModel
    {
        public int OrderId { get; set; }
        public string CustomerName { get; set; } = string.Empty;
        // De status als tekst (bijv. "In behandeling").
        public string StatusDescription { get; set; } = string.Empty;

        public Address DeliveryAddress { get; set; }

        public decimal TotalAmount { get; set; }
    }
}
