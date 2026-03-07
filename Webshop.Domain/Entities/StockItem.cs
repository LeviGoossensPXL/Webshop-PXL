using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Webshop.Domain.Entities
{
    public class StockItem
    {
        public int StockItemId { get; set; }
        public string sku { get; set; }
       public  int Quantity { get; set; }
       public  string WarehouseLocation { get; set; }

       public int ProductId { get; set; }
        public Product Product { get; set; }
    }
}
