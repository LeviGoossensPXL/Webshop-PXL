namespace Webshop.MVC.Models
{
    public class Order
    {
        public int OrderId { get; set; }
        public string CustomerId { get; set; }
        public int ProductId { get; set; }
        public int Quantiy { get; set; }
        public string DeliveryAddress { get; set; } //TODO: maybe different class?
        public int Status { get; set; }
    }
}
