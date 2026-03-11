using Webshop.Domain.Entities;

namespace Webshop.MVC.ViewModels
{
    
    public class OrderViewModel
    {
        public int OrderId { get; set; }
        public string CustomerName { get; set; } = string.Empty;
        // The status as text (e.g. "Pending").
        public string StatusDescription { get; set; } = string.Empty;

        public Address DeliveryAddress { get; set; }

        public decimal TotalAmount { get; set; }
    }
}
