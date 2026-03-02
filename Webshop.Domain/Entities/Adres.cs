using System;

namespace Webshop.Domain.Entities
{
    
    public class Adres
    {
        
        public string Straat { get; set; } = string.Empty;

        
        public string Huisnummer { get; set; } = string.Empty;

       
        public string Postcode { get; set; } = string.Empty;

       
        public string Stad { get; set; } = string.Empty;

        
        public string Land { get; set; } = "België";
    }
}
