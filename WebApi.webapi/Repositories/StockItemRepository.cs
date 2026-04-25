using Microsoft.EntityFrameworkCore;
using WebApi.Data;
using Webshop.Application.Repositories;
using Webshop.Domain.Entities;

namespace WebApi.Repositories;

public class StockItemRepository : IStockItemRepository
{
        private readonly AppDbContext _context;
        public StockItemRepository(AppDbContext context)
        {
            _context = context;
        }
        public async Task Add(StockItem stockItem)
        {
            _context.StockItems.Add(stockItem);
            await _context.SaveChangesAsync();
        }

        public async Task Delete(int id)
        {
            var existingStockItem = await _context.StockItems.FirstOrDefaultAsync(p => p.StockItemId == id);

            if (existingStockItem == null)
            {
                return;
            }

            _context.StockItems.Remove(existingStockItem);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<StockItem>> GetAll()
        {
            return await _context.StockItems.AsNoTracking().ToListAsync();
        }

        public async Task<StockItem?> GetById(int id)
        {
            return await _context.StockItems.AsNoTracking()
                .FirstOrDefaultAsync(stockItem => stockItem.StockItemId == id);
        }

        public async Task Update(StockItem stockItem)
        {
            _context.StockItems.Update(stockItem);
            await _context.SaveChangesAsync();
        }
    }