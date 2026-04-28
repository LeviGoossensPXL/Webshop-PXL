using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Webshop.Domain.Entities;
using WebApi.Data;
using WebApi.Dtos;
using Webshop.Application.Services.Contracts;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class StockItemController : ControllerBase
    {
        private readonly ILogger<StockItemController> _logger;
        private readonly AppDbContext _context;
        private readonly IStockItemService _stockItemService;

        public StockItemController(ILogger<StockItemController> logger, AppDbContext context, IStockItemService stockItemService)
        {
            _logger = logger;
            _context = context;
            _stockItemService = stockItemService;
        }

        // GET: /StockItem
        [HttpGet("/StockItem")]
        public async Task<IActionResult> GetAll()
        {
            var result = await _stockItemService.GetAll();
            return Ok(result.Data);
        }

        // GET: /StockItem/product/{productId}
        [HttpGet("/StockItem/product/{productId}")]
        public async Task<IActionResult> GetByProductId(int productId)
        {
            var result = await _stockItemService.GetByProductId(productId);
            if (!result.Succeeded)
            {
                return NotFound($"No stock found for ProductId {productId}");
            }
            return Ok(result.Data);
        }

        // POST: /StockItem
        [HttpPost("/StockItem")]
        public async Task<IActionResult> CreateStock([FromBody] CreateStockItemDto createStockItem)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            StockItem stockItem = new StockItem()
            {
                Quantity = createStockItem.Quantity,
                Sku = createStockItem.Sku,
                WarehouseLocation = createStockItem.WarehouseLocation,
                ProductId = createStockItem.ProductId
            };

            // Check if stock for this product already exists
            var result = await _stockItemService.Add(stockItem);
            if (!result.Succeeded)
            {
                return Conflict($"Stock already exists for ProductId {stockItem.ProductId}");
            }

            return Ok(stockItem);
        }

        // DELETE: /StockItem/product/{productId}
        [HttpDelete("/StockItem/product/{productId}")]
        public async Task<IActionResult> DeleteByProductId(int productId)
        {
            var result = await _stockItemService.DeleteByProductId(productId);
            if (!result.Succeeded)
            {
                return NotFound($"No stock found for ProductId {productId}");
            }
            return Ok($"Stock for ProductId {productId} deleted.");
        }

        // GET: /StockItem/{id}
        [HttpGet("/StockItem/{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _stockItemService.GetById(id);
            if (!result.Succeeded)
            {
                return NotFound($"No stock found with id {id}");
            }
            return Ok(result.Data);
        }

        // PUT: /StockItem/product/{productId}/reduce
        [HttpPut("/StockItem/product/{productId}/reduce")]
        public async Task<IActionResult> ReduceStock(int productId, [FromBody] int quantity)
        {
            var getResult = await _stockItemService.GetByProductId(productId);
            if (!getResult.Succeeded)
            {
                return NotFound($"No stock found for ProductId {productId}");
            }

            var stock = getResult.Data;
            if (stock.Quantity < quantity)
            {
                return BadRequest($"Insufficient stock. Available: {stock.Quantity}, Requested: {quantity}");
            }

            stock.Quantity -= quantity;
            var updateResult = await _stockItemService.Update(stock);
            if (!updateResult.Succeeded)
            {
                return StatusCode(500, "Failed to update stock.");
            }

            return Ok(stock);
        }

        // PUT: /StockItem/product/{productId}/add
        [HttpPut("/StockItem/product/{productId}/add")]
        public async Task<IActionResult> AddStock(int productId, [FromBody] int quantity)
        {
            var getResult = await _stockItemService.GetByProductId(productId);
            if (!getResult.Succeeded)
            {
                return NotFound($"No stock found for ProductId {productId}");
            }

            var stock = getResult.Data;
            stock.Quantity += quantity;
            var updateResult = await _stockItemService.Update(stock);
            if (!updateResult.Succeeded)
            {
                return StatusCode(500, "Failed to update stock.");
            }

            return Ok(stock);
        }

        // PUT: /StockItem/product/{productId}/set
        [HttpPut("/StockItem/product/{productId}/set")]
        public async Task<IActionResult> SetStock(int productId, [FromBody] int quantity)
        {
            var getResult = await _stockItemService.GetByProductId(productId);
            if (!getResult.Succeeded)
            {
                return NotFound($"No stock found for ProductId {productId}");
            }

            var stock = getResult.Data;
            stock.Quantity = quantity;
            var updateResult = await _stockItemService.Update(stock);
            if (!updateResult.Succeeded)
            {
                return StatusCode(500, "Failed to update stock.");
            }

            return Ok(stock);
        }
    }
}

