using System;
using System.Collections.Generic;

namespace Webshop.Domain.Entities
{
    public class Category
    {
        public int CategoryId { get; set; }
        public string Name { get; set; } = string.Empty;
        // Lijst met producten in deze categorie.
        public ICollection<Product> Products { get; set; } = new List<Product>();
    }
}
