using System;

namespace Webshop.MVC.Models
{
    public class OrderGetViewModel
    {
       
        public int OrderId { get; set; }

       
        public DateTime OrderDate { get; set; }

       
        // In de database is dit een int, maar hier tonen we tekst.
        public string Status { get; set; } = string.Empty;

        // Het totaal aantal items in de bestelling (berekend uit OrderLines)
        public int TotalItems { get; set; }

        // De totale prijs van de bestelling (berekend: Quantity * UnitPrice)
        public decimal TotalPrice { get; set; }

        // Het geformatteerde afleveradres (Straat + Stad + Postcode samen)
        public string FullAddress { get; set; } = string.Empty;
    }
}