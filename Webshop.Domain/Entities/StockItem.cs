using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Webshop.Domain.Entities
{
    public class StockItem
    {
        [Key]
        public int StockItemId { get; set; }
        public int Quantity { get; set; }
        public string? WarehouseLocation { get; set; }

        // Logical link to Product in the Webshop database (no FK, separate DB)
        public int ProductId { get; set; }
    }
}
