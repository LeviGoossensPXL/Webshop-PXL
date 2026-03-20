namespace Webshop.MVC.ViewModels
{
    public class ProductDetailViewModel
    {
        public int ProductId { get; set; }

        public string Name { get; set; } = string.Empty;

        public decimal Price { get; set; }

        public string Description { get; set; } = string.Empty;

        public string Sku { get; set; } = string.Empty;

        // We keep CategoryId just in case we need to create a link back to the category page from the product details page
        // to show all products in the same category so that the user can easily navigate to related products.
        public int CategoryId { get; set; }

        public string CategoryName { get; set; } = string.Empty;

        // Default image for products without an uploaded photo
        public string ImageUrl { get; set; } = "/images/default.jpg";
    }
}
