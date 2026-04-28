using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Webshop.Domain.Entities;
using WebApi.Data;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class StockItemController : ControllerBase
    {
        private readonly ILogger<StockItemController> _logger;
        private readonly AppDbContext _context;

        public StockItemController(ILogger<StockItemController> logger, AppDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        // GET: /StockItem
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var stocks = await _context.StockItems.ToListAsync();
            return Ok(stocks);
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

        // PUT: /StockItem/product/{productId}
        // Updates a stock item completely
        [HttpPut("product/{productId}")]
        public async Task<IActionResult> UpdateStock(int productId, [FromBody] StockItem updatedStock)
        {
            if (updatedStock == null || productId != updatedStock.ProductId)
            {
                return BadRequest("Invalid stock data.");
            }

            var stock = await _context.StockItems.FirstOrDefaultAsync(s => s.ProductId == productId);
            if (stock == null)
            {
                return NotFound($"No stock found for ProductId {productId}");
            }

            stock.Quantity = updatedStock.Quantity;
            stock.WarehouseLocation = updatedStock.WarehouseLocation;
            stock.Sku = updatedStock.Sku;

            await _context.SaveChangesAsync();

            return Ok(stock);
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
    }
}

