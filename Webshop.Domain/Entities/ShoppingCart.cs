using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Webshop.Domain.Entities
{
    public class ShoppingCart
    {
        // List of items in the shopping cart
        public List<ShoppingCartItem> Items { get; set; } = new List<ShoppingCartItem>();

        // Calculate the total price
        public decimal TotalPrice => Items.Sum(i => i.UnitPrice * i.Quantity);
    }
}
