using System;
using Microsoft.EntityFrameworkCore;
using Webshop.Domain.Entities;
using Webshop.Application.Repositories;
using Webshop.Infrastructure.Data;

namespace Webshop.Infrastructure.Repositories
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly AppDbContext _context;

        public CategoryRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Category>> GetAll()
        {
            return await _context.Categories.ToListAsync();
        }

        public async Task<Category?> GetById(int id)
        {
            return await _context.Categories.FindAsync(id);
        }
    }
}

