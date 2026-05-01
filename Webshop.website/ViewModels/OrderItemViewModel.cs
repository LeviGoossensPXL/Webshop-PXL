namespace Webshop.website.ViewModels
{
    public class OrderItemViewModel
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; } = string.Empty;
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }

        // Calculate the total price per line automatically
        public decimal TotalPrice => Quantity * UnitPrice;
    }
}
