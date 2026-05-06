using Microsoft.AspNetCore.Mvc;
using Webshop.Application.Services;
using Webshop.Application.Services.Contracts;
using Webshop.website.ViewModels;

namespace Webshop.website.Controllers
{
    // This controller is for the customer side
    public class ShopController : Controller
    {
        private readonly IShopService _shopService;
        private readonly IPageService _pageService;

        public ShopController(IShopService shopService, IPageService pageService)
        {
            _shopService = shopService;
            _pageService = pageService;
        }

        // GET: Shop/Index (De catalogus voor de klant met filters)
        [HttpGet]
        public async Task<IActionResult> Index1(int? categoryId, int page = 1)
        {
            var productsResult = await _shopService.GetProducts(categoryId, searchQuery);
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
            ViewBag.SearchQuery = searchQuery;

            return View(viewModelList);
        }

        // GET: Shop/Index (De catalogus voor de klant met filters)
        [HttpGet]
        public async Task<IActionResult> Index(int? categoryId, int page = 1)
        {
            var result = await _shopService.GetProducts(categoryId);

            var paging = _pageService.GetPaging(result.Data, page);

            var categories = await _shopService.GetCategories();

            var viewModelList = paging
                .list.Select(p => new ProductListViewModel
                {
                    Id = p.ProductId,
                    Name = p.Name,
                    Description = p.Description,
                    Price = p.Price,
                    ImageUrl = p.ImageUrl,
                    CategoryName = categories.FirstOrDefault(c => c.CategoryId == p.CategoryId)?.Name ?? "Unknown"
                });

            ViewBag.Categories = categories;

            return View(new ShopIndexViewModel
            {
                Products = viewModelList,
                PagingInfo = paging.pageInfo,
                CurrentCategory = categoryId
            });
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
