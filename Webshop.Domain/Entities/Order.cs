using System;
using System.Collections.Generic;


namespace Webshop.Domain.Entities
{
    public class Order
    {
        
        public int OrderId { get; set; }

      
        public string UserId { get; set; } = string.Empty;
        


        public Adres DeliveryAddress { get; set; } = new Adres();

        // Order status (e.g., 1 = Pending, 2 = Shipped)
        public int Status { get; set; }

       
        public DateTime OrderDate { get; set; } = DateTime.UtcNow;

        // List of specific items in this order
        public ICollection<OrderLine> OrderLines { get; set; } = new List<OrderLine>();
    }
}
