using System;

namespace Webshop.Domain.Entities
{
    // This class contains the details of each specific product in an order.
    public class OrderLine
    {
        public int OrderLineId { get; set; }

       
        public int OrderId { get; set; }
        public Order Order { get; set; } = null!;

        
        public int ProductId { get; set; }
        public Product Product { get; set; } = null!;

        // The number of items ordered by the customer.
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
    }
}
