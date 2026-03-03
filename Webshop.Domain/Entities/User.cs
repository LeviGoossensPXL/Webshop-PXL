using System;
using System.Collections.Generic;

namespace Webshop.Domain.Entities
{
    // Deze klasse stelt een gebruiker of klant voor in ons systeem.
    public class User
    {
        // De unieke Id van de gebruiker (vaak een string bij ASP.NET Identity).
        public string Id { get; set; } = Guid.NewGuid().ToString();

       
        public string Name { get; set; } = string.Empty;

        // Het e-mailadres voor inloggen en contact.
        public string Email { get; set; } = string.Empty;

        // Lijst van bestellingen die deze klant heeft geplaatst.
        public ICollection<Order> Orders { get; set; } = new List<Order>();
    }
}
