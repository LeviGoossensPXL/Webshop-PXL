using Webshop.Domain.Entities;

namespace Webshop.MVC.ViewModels
{
    public class OrderUpdateViewModel
    {
        public int OrderId { get; set; }

        public string? UserId { get; set; }

        public int? DeliveryAddress { get; set; }

        public int? Status { get; set; }

        public DateTime? OrderDate { get; set; }
    }
}
