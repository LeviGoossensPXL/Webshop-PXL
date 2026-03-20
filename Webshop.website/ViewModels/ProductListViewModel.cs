namespace Webshop.MVC.ViewModels
{
    // this model is used to show a list of campings products to the customer 
    public class ProductListViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public string? ImageUrl { get; set; } 

        public string? Description { get; set; }

        
        // in the product details page, the full description will be shown, so this is only for the product list page.
        public string ShortDescription 
        {
            get
            {
                if (string.IsNullOrEmpty(Description))
                {
                    return string.Empty;
                }
                if (Description.Length > 30)
                {
                    return Description.Substring(0, 30) + "...";
                }
                else
                {
                    return Description;
                }
            }
        }

        public string? CategoryName { get; set; }
    }
}
