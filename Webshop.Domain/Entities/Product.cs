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
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public string ImageUrl { get; set; }
        public string Sku { get; set; }

        // Link to the category.
        public int CategoryId { get; set; }
        public Category Category { get; set; }
    }
}
