namespace Webshop.MVC.ViewModels
{
    public class OrderListViewModel
    {
        public int OrderId { get; set; }
        public string UserId { get; set; }
        public DateTime OrderDate { get; set; }

        //We use string here to display the Enum as text (e.g., "Pending", "Shipped")
        public string Status { get; set; }
        public int NumberOfItems { get; set; }
        public decimal TotalAmount { get; set; }
    }
}
