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
        public string Naam { get; set; } = string.Empty;
        public decimal Prijs { get; set; }
        public int Voorraad { get; set; }
        public string Beschrijving { get; set; } = string.Empty;

        // Koppeling met de categorie.
        public int CategoryId { get; set; }
        public Category Category { get; set; } = null!;
    }
}
