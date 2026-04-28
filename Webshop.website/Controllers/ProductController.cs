using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;
using Webshop.Application.Repositories;
using Webshop.Application.Services.Contracts;
using Webshop.website.ViewModels;

namespace Webshop.website.Controllers
{
    [Authorize(Roles = "Admin")] // Only admins can access this controller
    public class ProductController : Controller
    {
        private readonly IProductService _productService;
        private readonly ICategoryRepository _categoryRepository;

        public ProductController(IProductService productService, ICategoryRepository categoryRepository)
        {
            _productService = productService;
            _categoryRepository = categoryRepository;
        }

        // GET: Product (GetAll)
        [HttpGet]
        public async Task<IActionResult> Index(int? categoryId)
        {
            var result = await _productService.GetAll(categoryId);
            var categories = await _categoryRepository.GetAll();

            var sortedData = result.Data
                .OrderBy(p => p.CategoryId)
                .ThenByDescending(p => p.ProductId);

            // Map Domain Entities to ViewModels (Using sorted data)
            var viewModelList = sortedData.Select(p => new ProductListViewModel
            {
                Id = p.ProductId,
                Name = p.Name,
                Description = p.Description,
                Price = p.Price,
                // If Category is not null, get its Name
                CategoryName = categories.FirstOrDefault(c => c.CategoryId == p.CategoryId)?.Name ?? "Unknown",
                ImageUrl = p.ImageUrl
            }).ToList();

            ViewBag.Categories = categories;
            ViewBag.CurrrentCategory = categoryId ?? 0;
            return View(viewModelList);
        }

        // GET: Product/Details/5 (Details - GetOne)
        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var result = await _productService.GetById(id);
            if (!result.Succeeded)
            {
                return NotFound();
            }

            // Map Domain Entity to Detail ViewModel
            var viewModel = new ProductDetailViewModel
            {
                ProductId = result.Data.ProductId,
                Name = result.Data.Name,
                Description = result.Data.Description,
                Price = result.Data.Price,
                Sku = result.Data.Sku,
                CategoryId = result.Data.CategoryId,
                CategoryName = result.Data.Category?.Name,
                ImageUrl = result.Data.ImageUrl
            };

            return View(viewModel);
        }

        // GET: Product/Create
        [HttpGet]
        public IActionResult Create()
        {
            // The Razor Component AddProduct handles everything now
            return View();
        }

        // GET: Product/Edit/5
        [HttpGet]
        public IActionResult Edit(int id)
        {
            // The Razor Component EditProduct fetches data and handles saving
            return View(id);
        }

        // GET: Product/Delete/5
        [HttpGet]
        public IActionResult Delete(int id)
        {
            // The Razor Component DeleteProduct fetches data and handles deletion
            return View(id);
        }
    }
}

