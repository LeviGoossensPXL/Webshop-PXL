using Microsoft.AspNetCore.Mvc;
using Webshop.Application.Services.Contracts;
using Webshop.Domain.Entities;
using Webshop.website.ViewModels;

namespace Webshop.website.Controllers
{
    // This controller is for the customer side
    public class ShopController : Controller
    {
        private readonly IProductService _productService;
        private readonly ICategoryService _categoryService;

        public ShopController(IProductService productService, ICategoryService categoryService)
        {
            _productService = productService;
            _categoryService = categoryService;
        }

        // GET: Shop/Index (De catalogus voor de klant met filters)
        [HttpGet]
        public async Task<IActionResult> Index(int? categoryId)
        {
            var productResult = await _productService.GetAll();
            var categories = await _categoryService.GetAll();

            // Filter products if a specific category is selected
            var products = productResult.Data ?? new List<Product>();

            var viewModelList = products.Select(p => new ProductListViewModel
            {
                Id = p.ProductId,
                Name = p.Name,
                Description = p.Description,
                Price = p.Price,
                ImageUrl = p.ImageUrl,
                CategoryName = categories.FirstOrDefault(c => c.CategoryId == p.CategoryId)?.Name ?? "Unknown"
            }).ToList();

            // Send data to the view for the category filter buttons
            ViewBag.Categories = categories;
            ViewBag.CurrentCategory = categoryId ?? 0;

            return View(viewModelList);
        }

        // GET: Shop/Details/5 (Product details for the customer)
        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var productResult = await _productService.GetById(id);
            if (productResult == null || !productResult.Succeeded) return NotFound();

            var categories = await _categoryService.GetAll();
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