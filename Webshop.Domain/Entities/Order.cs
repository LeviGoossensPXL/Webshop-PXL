using System;
using System.Collections.Generic;


namespace Webshop.Domain.Entities
{
    public class Order
    {
        
        public int OrderId { get; set; }

        
        public string UserId { get; set; }

        public Address DeliveryAddress { get; set; }

        // Order status (e.g., 1 = Pending, 2 = Shipped)
        public OrderStatus Status { get; set; } = OrderStatus.Pending; // Default status upon creation


        public DateTime OrderDate { get; set; } = DateTime.UtcNow;

        // List of specific items in this order
        public ICollection<OrderLine> OrderLines { get; set; } = new List<OrderLine>();
    }
}
