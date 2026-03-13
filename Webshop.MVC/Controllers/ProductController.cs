using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Linq;
using Webshop.Application.Repositories;
using Webshop.Domain.Entities;
using Webshop.MVC.ViewModels;

namespace Webshop.MVC.Controllers
{
    public class ProductController : Controller
    {
        private readonly IProductRepository _productRepository;
        private readonly ICategoryRepository _categoryRepository;

        public ProductController(IProductRepository productRepository, ICategoryRepository categoryRepository)
        {
            _productRepository = productRepository;
            _categoryRepository = categoryRepository;
        }

        // GET: Product (GetAll)
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var products = await _productRepository.GetAll();
            
            // Map Domain Entities to ViewModels
            var viewModelList = products.Select(p => new ProductListViewModel
            {
                Id = p.ProductId,
                Name = p.Name,
                Description = p.Description,
                Price = p.Price,
                // If Category is not null, get its Name
                CategoryName=p.Category?.Name,
                ImageUrl = p.ImageUrl
            }).ToList();

            return View(viewModelList);
        }

        // GET: Product/Details/5 (Details - GetOne)
        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var product = await _productRepository.GetById(id);
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
                CategoryName = product.Category.Name,
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
                    CategoryId = model.CategoryId,
                    // If ImageUrl is empty or consists only of spaces, print the default image
                    ImageUrl = string.IsNullOrWhiteSpace(model.ImageUrl)
                            ? "/images/default.jpg"
                            : model.ImageUrl
                };

                await _productRepository.Add(product);
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
            var product = await _productRepository.GetById(id);
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
                
                string imageUrl = model.CurrentImageUrl ?? "/images/default.jpg";

               
                if (model.NewImage != null && model.NewImage.Length > 0)
                {
                    
                    string fileName = Guid.NewGuid().ToString() + Path.GetExtension(model.NewImage.FileName);

                    // Define the path to save the file in wwwroot/images
                    string filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images", fileName);

                    
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await model.NewImage.CopyToAsync(stream);
                    }

                    
                    imageUrl = "/images/" + fileName;
                }

                var product = new Product
                {
                    ProductId = model.Id,
                    Name = model.Name,
                    Description = model.Description,
                    Price = model.Price,
                    Sku = model.Sku,
                    CategoryId = model.CategoryId,
                    ImageUrl = imageUrl // Use either the old one or the newly uploaded one
                };

                await _productRepository.Update(product);
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
            var product = await _productRepository.GetById(id);
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
            var product = await _productRepository.GetById(id);

            if (product != null)
            {
                // 1. Physical file cleanup (Don't delete the default image!)
                if (!string.IsNullOrEmpty(product.ImageUrl) && product.ImageUrl != "/images/default.jpg")
                {
                    var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", product.ImageUrl.TrimStart('/'));

                    if (System.IO.File.Exists(filePath))
                    {
                        System.IO.File.Delete(filePath);
                    }
                }

                // 2. Database cleanup
                await _productRepository.Delete(id);
            }

            return RedirectToAction(nameof(Index));
        }

    }
}
