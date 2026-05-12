using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Json;
using Webshop.Application.Services.Contracts;
using Webshop.Domain.Entities;
using Webshop.website.Mappings;
using Webshop.website.ViewModels;

namespace Webshop.website.Controllers;

[Authorize(Roles = "Admin")]
public class StockController : Controller
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly IProductService _productService;
    private readonly StockMapper _mapper = new();
    private readonly ICategoryService _categoryService;

    public StockController(IHttpClientFactory httpClientFactory, IProductService productService, ICategoryService categoryService)
    {
        _httpClientFactory = httpClientFactory;
        _productService = productService;
        _categoryService = categoryService;
    }

    // GET: Stock
    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var client = _httpClientFactory.CreateClient("StockApi");
        var stockItems = await client.GetFromJsonAsync<List<StockItem>>("/StockItem") ?? new List<StockItem>();

        var productsResult = await _productService.GetAll(null);
        var productMap = productsResult.Data.ToDictionary(p => p.ProductId, p => p.Name);
        var categories = await _categoryService.GetAll();
        var viewModelList = stockItems.Select(s =>
        {
            var vm = _mapper.ToListViewModel(s);
            var prod = productsResult.Data.First(p => p.ProductId == s.ProductId);
            vm.Sku = prod.Sku;
            vm.Category = categories.First(category => category.CategoryId == prod.CategoryId).Name;
            vm.ProductName = productMap.TryGetValue(s.ProductId, out var name) ? name : $"Product #{s.ProductId}";
            return vm;
        }).OrderBy(s => s.ProductName).ToList();

        return View(viewModelList);
    }

    // GET: Stock/Edit/5 (by ProductId)
    [HttpGet]
    public async Task<IActionResult> Edit(int id)
    {
        var client = _httpClientFactory.CreateClient("StockApi");
        var stockItem = await client.GetFromJsonAsync<StockItem>($"/StockItem/product/{id}");

        if (stockItem == null)
            return NotFound();

        var productResult = await _productService.GetById(id);
        var productName = productResult.Succeeded ? productResult.Data.Name : $"Product #{id}";

        var model = _mapper.ToEditViewModel(stockItem);
        model.Sku = productResult.Data.Sku;
        model.ProductName = productName;

        return View(model);
    }

    // POST: Stock/Edit/5
    [HttpPost]
    public async Task<IActionResult> Edit(StockEditViewModel model)
    {
        if (!ModelState.IsValid)
            return View(model);
        var productResult = await _productService.GetById(model.ProductId);
        if (!productResult.Succeeded)
        {
            ModelState.AddModelError(string.Empty, "No product found with that info.");
            return View(model);
        }

        var stockItem = _mapper.ToStockItem(model);

        var client = _httpClientFactory.CreateClient("StockApi");
        var response = await client.PutAsJsonAsync($"/StockItem/product/{model.ProductId}", stockItem);

        if (!response.IsSuccessStatusCode)
        {
            ModelState.AddModelError(string.Empty, "Failed to update stock. Please try again.");
            return View(model);
        }
        productResult.Data.Sku = model.Sku!;
        var updateProductResult = await _productService.Update(productResult.Data, null, null);
        if (!updateProductResult.Succeeded)
        {
            ModelState.AddModelError(string.Empty, "Failed to update stock. Please try again.");
            return View(model);
        }

        return RedirectToAction(nameof(Index));
    }
}