namespace Webshop.website.ViewModels
{
    public class OrderCreateViewModel
    {
        public string Street { get; set; }
        public string HouseNumber { get; set; }
        public string City { get; set; }
        public string PostalCode { get; set; }
        public string Country { get; set; }
        public decimal TotalAmount { get; set; }
    }
}
