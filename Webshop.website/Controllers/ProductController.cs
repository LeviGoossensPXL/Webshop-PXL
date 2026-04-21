using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Linq;
using System.Net.Http.Json;
using Webshop.Application.Services.Contracts;
using Webshop.Domain.Entities;
using Webshop.website.ViewModels;

namespace Webshop.website.Controllers
{
    [Authorize(Roles = "Admin")] // Only admins can access this controller
    public class ProductController : Controller
    {
        private readonly IProductService _productService;
        private readonly ICategoryService _categoryService;
        private readonly IHttpClientFactory _httpClientFactory;

        public ProductController(IProductService productService, ICategoryService categoryService, IHttpClientFactory httpClientFactory)
        {
            _productService = productService;
            _categoryService = categoryService;
            _httpClientFactory = httpClientFactory;
        }

        // GET: Product (GetAll)
        [HttpGet]
        public async Task<IActionResult> Index(int? categoryId)
        {
            var result = await _productService.GetAll(categoryId);
            var categories = await _categoryService.GetAll();

             
            // Map Domain Entities to ViewModels
            var viewModelList = result.Data.Select(p => new ProductListViewModel
            {
                Id = p.ProductId,
                Name = p.Name,
                Description = p.Description,
                Price = p.Price,
                // If Category is not null, get its Name
                CategoryName = categories.FirstOrDefault(c =>c.CategoryId ==p.CategoryId)?.Name ?? "Unknown",
                ImageUrl = p.ImageUrl
            }).ToList();

            ViewBag.Categories = categories;
            ViewBag.CurrrentCategory= categoryId ?? 0 ;
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
        // GET: Product/Create (create form to open )
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            // Get categories for the dropdown menu
            var categories = await _categoryService.GetAll();
            ViewBag.Categories = new SelectList(categories, "CategoryId", "Name");

            return View();
        }

        // POST: Product/Create (for the form save )
        [HttpPost]
        public async Task<IActionResult> Create(ProductCreateViewModel model)
        {
            if (ModelState.IsValid)
            {
                // Map ViewModel to Domain Entity
                var product = new Product
                {
                    Name = model.Name,
                    Description = model.Description,
                    Price = model.Price,
                    Sku = model.Sku,
                    CategoryId = model.CategoryId
                };

                var result = await _productService.Add(product, model.ImageUrl);
                if (result.Succeeded)
                {
                    // Create stock entry in the Stock Web API
                    try
                    {
                        var client = _httpClientFactory.CreateClient("StockApi");
                        var stockItem = new StockItem
                        {
                            ProductId = product.ProductId,
                            Quantity = model.InitialStock,
                            Sku = product.Sku ?? "NO-SKU",
                            WarehouseLocation = string.IsNullOrWhiteSpace(model.WarehouseLocation) ? "Default Warehouse" : model.WarehouseLocation
                        };
                        await client.PostAsJsonAsync("/StockItem", stockItem);
                    }
                    catch (Exception ex)
                    {
                        // Log but don't block — product is already saved
                        Console.WriteLine($"Stock API call failed: {ex.Message}");
                    }

                    return RedirectToAction("Index");
                }

                ModelState.AddModelError(string.Empty, result.Errors.FirstOrDefault() ?? "An error occurred.");
            }

            // If there is a validation error, reload the categories and show the form again
            var categories = await _categoryService.GetAll();
            ViewBag.Categories = new SelectList(categories, "CategoryId", "Name");
            return View(model);
        }
        // GET: Product/Edit/5 (edit form to open )
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var result = await _productService.GetById(id);
            if (!result.Succeeded)
            {
                return NotFound();
            }

            // Map Domain Entity to Update ViewModel
            var model = new ProductUpdateViewModel
            {
                Id = result.Data.ProductId, 
                Name = result.Data.Name,
                Description = result.Data.Description,
                Price = result.Data.Price,
                Sku = result.Data.Sku,
                CategoryId = result.Data.CategoryId,
                CurrentImageUrl = result.Data.ImageUrl
            };

            // Get categories for the dropdown menu
            var categories = await _categoryService.GetAll();
            ViewBag.Categories = new SelectList(categories, "CategoryId", "Name");

            return View(model);
        }

        // POST: Product/Edit/5 (Save edit)
        [HttpPost]
        public async Task<IActionResult> Edit(ProductUpdateViewModel model)
        {
            if (ModelState.IsValid)
            {
                var product = new Product
                {
                    ProductId = model.Id,
                    Name = model.Name,
                    Description = model.Description,
                    Price = model.Price,
                    Sku = model.Sku,
                    CategoryId = model.CategoryId
                };

                var result = await _productService.Update(product, model.NewImage, model.CurrentImageUrl);

                if (result.Succeeded)
                {
                    return RedirectToAction("Index");
                }

                ModelState.AddModelError(string.Empty, result.Errors.FirstOrDefault() ?? "An error occurred.");
            }

            // If error, reload categories for the dropdown
            var categories = await _categoryService.GetAll();
            ViewBag.Categories = new SelectList(categories, "CategoryId", "Name");
            return View(model);
        }

        //GET: Product/Delete/5 (delete confirm page)
        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _productService.GetById(id);
            if (!result.Succeeded)
            {
                return NotFound();
            }

            var viewModel = new ProductDetailViewModel
            {
                ProductId = result.Data.ProductId,
                Name = result.Data.Name,
                Description = result.Data.Description,
                Price = result.Data.Price,
                ImageUrl = result.Data.ImageUrl
            };

            return View(viewModel); // We show the details to ask "Are you sure?"
        }

        // POST: Product/Delete/5 
        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _productService.Delete(id);

            // Also delete the stock entry from the Stock Web API
            try
            {
                var client = _httpClientFactory.CreateClient("StockApi");
                await client.DeleteAsync($"/StockItem/product/{id}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Stock API delete failed: {ex.Message}");
            }

            return RedirectToAction(nameof(Index));
        }

    }
}
