using Webshop.Application.Helpers;

namespace Webshop.website.ViewModels
{
    public class ShopIndexViewModel
    {
        public IEnumerable<ProductListViewModel> Products { get; set; } = [];
        public PagingInfo PagingInfo { get; set; }
        public int? CurrentCategory { get; set; }
    }
}
