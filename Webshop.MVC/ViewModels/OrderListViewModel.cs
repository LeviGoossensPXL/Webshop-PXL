namespace Webshop.MVC.ViewModels
{
    public class OrderListViewModel
    {
        public int OrderId { get; set; }
        public string UserId { get; set; }
        public DateTime OrderDate { get; set; }
        public int Status { get; set; }
        public int NumberOfItems { get; set; }
        public decimal TotalAmount { get; set; }
    }
}
