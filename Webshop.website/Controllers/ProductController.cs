using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Linq;
using Webshop.Application.Repositories;
using Webshop.Application.Services.Contracts;
using Webshop.Domain.Entities;
using Webshop.website.ViewModels;

namespace Webshop.website.Controllers
{
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
            var products = await _productService.GetAll(categoryId);
            var categories = await _categoryRepository.GetAll();

             
            // Map Domain Entities to ViewModels
            var viewModelList = products.Select(p => new ProductListViewModel
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
            var product = await _productService.GetById(id);
            if (product == null)
            {
                return NotFound();
            }

            // Map Domain Entity to Detail ViewModel
            var viewModel = new ProductDetailViewModel
            {
                ProductId = product.ProductId,
                Name = product.Name,
                Description = product.Description,
                Price = product.Price,
                Sku = product.Sku,
                CategoryId = product.CategoryId,
                CategoryName = product.Category?.Name,
                ImageUrl = product.ImageUrl
            };

            return View(viewModel);
        }
        // GET: Product/Create (create form to open )
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            // Get categories for the dropdown menu
            var categories = await _categoryRepository.GetAll();
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

                await _productService.Add(product, model.ImageUrl);
                return RedirectToAction("Index"); // Return to list after saving
            }

            // If there is a validation error, reload the categories and show the form again
            var categories = await _categoryRepository.GetAll();
            ViewBag.Categories = new SelectList(categories, "CategoryId", "Name");
            return View(model);
        }
        // GET: Product/Edit/5 (edit form to open )
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var product = await _productService.GetById(id);
            if (product == null)
            {
                return NotFound();
            }

            // Map Domain Entity to Update ViewModel
            var model = new ProductUpdateViewModel
            {
                Id = product.ProductId, 
                Name = product.Name,
                Description = product.Description,
                Price = product.Price,
                Sku = product.Sku,
                CategoryId = product.CategoryId,
                CurrentImageUrl = product.ImageUrl
            };

            // Get categories for the dropdown menu
            var categories = await _categoryRepository.GetAll();
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

                await _productService.Update(product, model.NewImage, model.CurrentImageUrl);
                return RedirectToAction("Index");
            }

            // If error, reload categories for the dropdown
            var categories = await _categoryRepository.GetAll();
            ViewBag.Categories = new SelectList(categories, "CategoryId", "Name");
            return View(model);
        }

        //GET: Product/Delete/5 (delete confirm page)
        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var product = await _productService.GetById(id);
            if (product == null)
            {
                return NotFound();
            }

            var viewModel = new ProductDetailViewModel
            {
                ProductId = product.ProductId,
                Name = product.Name,
                Description = product.Description,
                Price = product.Price,
                ImageUrl = product.ImageUrl
            };

            return View(viewModel); // We show the details to ask "Are you sure?"
        }

        // POST: Product/Delete/5 
        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _productService.Delete(id);
            return RedirectToAction(nameof(Index));
        }

    }
}
