using Microsoft.AspNetCore.Mvc;
using Webshop.Application.Services.Contracts;
using Webshop.website.ViewModels;

namespace Webshop.website.Controllers
{
    // This controller is for the customer side
    public class ShopController : Controller
    {
        private readonly IShopService _shopService;

        public ShopController(IShopService shopService)
        {
            _shopService = shopService;
        }

        // GET: Shop/Index (De catalogus voor de klant met filters)
        [HttpGet]
        public async Task<IActionResult> Index(int? categoryId)
        {
            var productsResult = await _shopService.GetProducts(categoryId);
            var categories = await _shopService.GetCategories();

            var viewModelList = productsResult.Data.Select(p => new ProductListViewModel
            {
                Id = p.ProductId,
                Name = p.Name,
                Description = p.Description,
                Price = p.Price,
                ImageUrl = p.ImageUrl,
                CategoryName = categories.FirstOrDefault(c => c.CategoryId == p.CategoryId)?.Name ?? "Unknown"
            }).ToList();

            ViewBag.Categories = categories;
            ViewBag.CurrentCategory = categoryId ?? 0;

            return View(viewModelList);
        }

        // GET: Shop/Details/5 (Product details for the customer)
        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var productResult = await _shopService.GetProductById(id);
            if (!productResult.Succeeded) return NotFound();

            var categories = await _shopService.GetCategories();
            var product = productResult.Data;

            var viewModel = new ProductDetailViewModel
            {
                ProductId = product.ProductId,
                Name = product.Name,
                Description = product.Description,
                Price = product.Price,
                ImageUrl = product.ImageUrl,
                CategoryName = categories.FirstOrDefault(c => c.CategoryId == product.CategoryId)?.Name ?? "Onbekend"
            };

            return View(viewModel);
        }
    }
}
