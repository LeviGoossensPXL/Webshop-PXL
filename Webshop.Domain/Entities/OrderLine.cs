using System;

namespace Webshop.Domain.Entities
{
    // Deze klasse bevat de details van elk specifiek product in een bestelling.
    public class OrderLine
    {
        public int OrderLineId { get; set; }

       
        public int OrderId { get; set; }
        public Order Order { get; set; } = null!;

        
        public int ProductId { get; set; }
        public Product Product { get; set; } = null!;

        // Het aantal stuks dat de klant bestelt.
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
    }
}
