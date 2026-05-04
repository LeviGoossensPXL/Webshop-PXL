using Webshop.Domain.Entities;
using System.Collections.Generic;

namespace Webshop.website.ViewModels
{
    public class OrderUpdateViewModel
    {
        public int OrderId { get; set; }

        public string? UserId { get; set; }
        public string? UserEmail { get; set; }

        public string? FullAddress { get; set; }

        public int? Status { get; set; }

        public DateTime? OrderDate { get; set; }

        public List<OrderLineViewModel> OrderLines { get; set; } = new List<OrderLineViewModel>();
        public decimal TotalAmount { get; set; }
    }
}