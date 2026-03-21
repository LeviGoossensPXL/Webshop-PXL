using System;

namespace Webshop.website.ViewModels
{
    // This model is used to show the full details of a specific order
    public class OrderDetailViewModel
    {
        public int OrderId { get; set; }

        public string UserId { get; set; } = string.Empty;

        public DateTime OrderDate { get; set; }

        // We use string here to display the Enum as text (e.g., "Pending", "Shipped")
        public string Status { get; set; } = string.Empty;

       
        public int TotalItems { get; set; }

  
        public decimal TotalPrice { get; set; }

        // The formatted delivery address (Street + City + PostalCode together)
        public string FullAddress { get; set; } = string.Empty;
    }
}