using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Webshop.Domain.Entities
{
    public enum OrderStatus
    {
        Pending = 1,    // In afwachting
        Processing = 2, // In behandeling
        Shipped = 3,    // Verzonden
        Delivered = 4,  // Geleverd
        Cancelled = 5   // Geannuleerd
    }
}
