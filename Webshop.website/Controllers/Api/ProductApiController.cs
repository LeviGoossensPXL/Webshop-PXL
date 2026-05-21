using Microsoft.AspNetCore.Mvc;
using Webshop.Application.Services.Contracts;
using Webshop.Domain.Entities;

namespace Webshop.website.Controllers.Api
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductApiController : ControllerBase
    {
        private readonly IProductService _productService;
        private readonly ICategoryService _categoryService;

        public ProductApiController(IProductService productService, ICategoryService categoryService)
        {
            _productService = productService;
            _categoryService = categoryService;
        }

        [HttpGet("categories")]
        public async Task<ActionResult<IEnumerable<Category>>> GetCategoriesAsync()
        {
            try
            {
                var categories = await _categoryService.GetAll();
                return Ok(categories);
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpPost]
        public async Task<ActionResult<Product>> PostAsync([FromBody] Product product)
        {
            var result = await _productService.Add(product, product.ImageUrl);
            if (result.Succeeded)
            {
                return Ok(product);
            }

            return BadRequest(result.Errors);
        }
    }
}
