namespace Webshop.website.ViewModels
{
    public class ShopIndexViewModel
    {
        public IEnumerable<ProductListViewModel> Products { get; set; } = [];
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
        public int? CurrentCategory { get; set; }
    }
}
