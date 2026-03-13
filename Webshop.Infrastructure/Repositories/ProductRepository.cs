using Microsoft.EntityFrameworkCore;
using Webshop.Application.Repositories;
using Webshop.Domain.Entities;
using Webshop.Infrastructure.Data;

namespace Webshop.Infrastructure.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly AppDbContext _context;
        public ProductRepository(AppDbContext context)
        {
            _context = context;
        }
        public async Task Add(Product product)
        {
            _context.Products.Add(product);
            await _context.SaveChangesAsync();
        }

        public async Task Delete(int id)
        {
            // Check if the entity is already being tracked in the local memory
            var existingProduct = _context.Products.Local.FirstOrDefault(p => p.ProductId == id);

            // If not in memory, fetch it from the database
            if (existingProduct == null)
            {
                existingProduct = await _context.Products.FindAsync(id);
            }

            if (existingProduct != null)
            {
                _context.Products.Remove(existingProduct);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<Product>> GetAll()
        {
            return await _context.Products.AsNoTracking().ToListAsync();
        }

        public async Task<Product?> GetById(int id)
        {
            // Include the Category navigation property to load the related category data
            // otherwise,the Category property will be null when accessed outside of this method
            return await _context.Products
                         .Include(p => p.Category)
                         .FirstOrDefaultAsync(p => p.ProductId == id);
        }

        public async Task Update(Product product)
        {
            _context.Products.Update(product);
            await _context.SaveChangesAsync();
        }
    }
}
