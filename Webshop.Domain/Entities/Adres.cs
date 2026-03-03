using System;

namespace Webshop.Domain.Entities
{
    
    public class Adres
    {
        
        public string Street { get; set; } = string.Empty;

        
        public string HouseNumber { get; set; } = string.Empty;

       
        public string ZipCode { get; set; } = string.Empty;

       
        public string City { get; set; } = string.Empty;

        
        public string Country { get; set; } = "België";
    }
}
