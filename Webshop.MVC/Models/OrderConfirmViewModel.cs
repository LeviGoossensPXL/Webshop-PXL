using System;

namespace Webshop.MVC.Models
{
    // This model displays the summary of an order to the customer.
    public class OrderConfirmViewModel
    {
        public int OrderId { get; set; }
        public DateTime Date { get; set; }
        public decimal Total { get; set; }
        public string CustomerName { get; set; } = string.Empty;
    }
}
