using Microsoft.AspNetCore.Mvc;
using Webshop.Application.Repositories;
using Webshop.website.ViewModels;

namespace Webshop.website.Controllers
{
    // This controller is for the customer side
    public class ShopController : Controller
    {
        private readonly IProductRepository _productRepository;
        private readonly ICategoryRepository _categoryRepository;

        public ShopController(IProductRepository productRepository, ICategoryRepository categoryRepository)
        {
            _productRepository = productRepository;
            _categoryRepository = categoryRepository;
        }

        // GET: Shop/Index (De catalogus voor de klant met filters)
        [HttpGet]
        public async Task<IActionResult> Index(int? categoryId)
        {
            var products = await _productRepository.GetAll();
            var categories = await _categoryRepository.GetAll();

            // Filter products if a specific category is selected
            if (categoryId.HasValue && categoryId > 0)
            {
                products = products.Where(p => p.CategoryId == categoryId.Value);
            }

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
            var product = await _productRepository.GetById(id);
            if (product == null) return NotFound();

            var categories = await _categoryRepository.GetAll();

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