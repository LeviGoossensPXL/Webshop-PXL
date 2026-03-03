using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Webshop.Domain.Entities;


namespace Webshop.Domain.Entities
{
    
    public class Product
    {
        public int ProductId { get; set; }
        public string Name { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public int Stock { get; set; }
        public string Description { get; set; } = string.Empty;

        // Koppeling met de categorie.
        public int CategoryId { get; set; }
        public Category Category { get; set; } = null!;
    }
}
