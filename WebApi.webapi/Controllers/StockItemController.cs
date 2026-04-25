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
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var stocks = await _context.StockItems.ToListAsync();
            return Ok(stocks);
        }

        // GET: /StockItem
        [HttpGet("/StockItem1")]
        public async Task<IActionResult> GetAll1()
        {
            var result = await _stockItemService.GetAll();
            return Ok(result.Data);
        }

        // GET: /StockItem/product/{productId}
        [HttpGet("product/{productId}")]
        public async Task<IActionResult> GetByProductId(int productId)
        {
            var stock = await _context.StockItems.FirstOrDefaultAsync(s => s.ProductId == productId);
            if (stock == null)
            {
                return NotFound($"No stock found for ProductId {productId}");
            }
            return Ok(stock);
        }

        // GET: /StockItem/product/{productId}
        [HttpGet("product1/{productId}")]
        public async Task<IActionResult> GetByProductId1(int productId)
        {
            var result = await _stockItemService.GetByProductId(productId);
            if (!result.Succeeded)
            {
                return NotFound($"No stock found for ProductId {productId}");
            }
            return Ok(result.Data);
        }

        // POST: /StockItem
        [HttpPost]
        public async Task<IActionResult> CreateStock([FromBody] StockItem stockItem)
        {
            if (stockItem == null)
            {
                return BadRequest("Stock data is empty.");
            }

            // Check if stock for this product already exists
            var existing = await _context.StockItems.FirstOrDefaultAsync(s => s.ProductId == stockItem.ProductId);
            if (existing != null)
            {
                return Conflict($"Stock already exists for ProductId {stockItem.ProductId}");
            }

            _context.StockItems.Add(stockItem);
            await _context.SaveChangesAsync();

            return Ok(stockItem);
        }

        // POST: /StockItem
        [HttpPost("/StockItem1")]
        public async Task<IActionResult> CreateStock1([FromBody] CreateStockItemDto createStockItem)
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

        // PUT: /StockItem/product/{productId}/reduce
        // Reduces stock quantity (e.g., when an order is placed)
        [HttpPut("product/{productId}/reduce")]
        public async Task<IActionResult> ReduceStock(int productId, [FromBody] int quantity)
        {
            var stock = await _context.StockItems.FirstOrDefaultAsync(s => s.ProductId == productId);
            if (stock == null)
            {
                return NotFound($"No stock found for ProductId {productId}");
            }

            if (stock.Quantity < quantity)
            {
                return BadRequest($"Insufficient stock. Available: {stock.Quantity}, Requested: {quantity}");
            }

            stock.Quantity -= quantity;
            await _context.SaveChangesAsync();

            return Ok(stock);
        }

        // PUT: /StockItem/product/{productId}/add
        // Increases stock quantity (e.g., when an order is cancelled)
        [HttpPut("product/{productId}/add")]
        public async Task<IActionResult> AddStock(int productId, [FromBody] int quantity)
        {
            var stock = await _context.StockItems.FirstOrDefaultAsync(s => s.ProductId == productId);
            if (stock == null)
            {
                return NotFound($"No stock found for ProductId {productId}");
            }

            stock.Quantity += quantity;
            await _context.SaveChangesAsync();

            return Ok(stock);
        }

        // PUT: /StockItem/product/{productId}/set
        // Sets stock quantity to a specific value (e.g., admin override)
        [HttpPut("product/{productId}/set")]
        public async Task<IActionResult> SetStock(int productId, [FromBody] int quantity)
        {
            var stock = await _context.StockItems.FirstOrDefaultAsync(s => s.ProductId == productId);
            if (stock == null)
            {
                return NotFound($"No stock found for ProductId {productId}");
            }

            stock.Quantity = quantity;
            await _context.SaveChangesAsync();

            return Ok(stock);
        }

        // DELETE: /StockItem/product/{productId}
        [HttpDelete("product/{productId}")]
        public async Task<IActionResult> DeleteByProductId(int productId)
        {
            var stock = await _context.StockItems.FirstOrDefaultAsync(s => s.ProductId == productId);
            if (stock == null)
            {
                return NotFound($"No stock found for ProductId {productId}");
            }

            _context.StockItems.Remove(stock);
            await _context.SaveChangesAsync();

            return Ok($"Stock for ProductId {productId} deleted.");
        }

        // DELETE: /StockItem/product/{productId}
        [HttpDelete("product1/{productId}")]
        public async Task<IActionResult> DeleteByProductId1(int productId)
        {
            var result = await _stockItemService.DeleteByProductId(productId);
            if (!result.Succeeded)
            {
                return NotFound($"No stock found for ProductId {productId}");
            }
            return Ok($"Stock for ProductId {productId} deleted.");
        }

        // GET: /StockItem/{id}
        [HttpGet("{id}")]
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
        [HttpPut("product1/{productId}/reduce")]
        public async Task<IActionResult> ReduceStock1(int productId, [FromBody] int quantity)
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
        [HttpPut("product1/{productId}/add")]
        public async Task<IActionResult> AddStock1(int productId, [FromBody] int quantity)
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
        [HttpPut("product1/{productId}/set")]
        public async Task<IActionResult> SetStock1(int productId, [FromBody] int quantity)
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

