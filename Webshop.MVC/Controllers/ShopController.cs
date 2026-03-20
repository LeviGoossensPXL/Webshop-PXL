using Microsoft.AspNetCore.Mvc;
using Webshop.Application.Repositories;
using Webshop.MVC.ViewModels;

namespace Webshop.MVC.Controllers
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

        // GET: Shop/Index (The customer catalogue)
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            // Retrieve all products (we're ignoring CRUD operations for customers)
            var products = await _productRepository.GetAll();
            var categories = await _categoryRepository.GetAll();

            var viewModelList = products.Select(p => new ProductListViewModel
            {
                Id = p.ProductId,
                Name = p.Name,
                Description = p.Description,
                Price = p.Price,
                ImageUrl = p.ImageUrl,
                CategoryName = categories.FirstOrDefault(c => c.CategoryId == p.CategoryId)?.Name ?? "Onbekend"
            }).ToList();

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