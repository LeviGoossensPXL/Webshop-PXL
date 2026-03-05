using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Webshop.Application.Repositories;
using Webshop.Domain.Entities;
using Webshop.Infrastructure.Data;

namespace Webshop.Infrastructure.Repositories
{
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
            var item = await _context.StockItems.FindAsync(id);
            if ((item != null))
            {
                _context.StockItems.Remove(item);
                await _context.SaveChangesAsync();
            }
        }

        public async  Task<IEnumerable<StockItem>> GetAll()
        {
            return await _context.StockItems.ToListAsync();
        }

        public async Task<StockItem?> GetById(int id)
        {
            return await _context.StockItems.FindAsync(id);
        }

        public async Task Update(StockItem stockItem)
        {
            _context.StockItems.Update(stockItem);
            await _context.SaveChangesAsync();
        }
    }
}
