using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Webshop.Domain.Entities
{
    public class StockItem
    {
        int StockItemId { get; set; }
        string sku { get; set; }
        int Quantity { get; set; }
        string WarehouseLocation { get; set; }
    }
}
