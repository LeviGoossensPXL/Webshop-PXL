using System;

namespace Webshop.MVC.Models
{
    public class OrderGetViewModel
    {
       
        public int OrderId { get; set; }

       
        public DateTime OrderDate { get; set; }


        // In the database, this is an int, but here we display text.
        public string Status { get; set; } = string.Empty;

        // The total number of items in the order (calculated from OrderLines)
        public int TotalItems { get; set; }

        // The total price of the order (calculated: Quantity * UnitPrice)
        public decimal TotalPrice { get; set; }

        // The formatted delivery address (Street + Town + Postcode together)
        public string FullAddress { get; set; } = string.Empty;
    }
}