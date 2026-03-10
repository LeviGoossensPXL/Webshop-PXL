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
    public class OrderLineRepository : IOrderLineRepository
    {
        private readonly AppDbContext _context;
        public OrderLineRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task Add(OrderLine orderLine)
        {
            _context.OrderLines.Add(orderLine);
            await _context.SaveChangesAsync();
        }

        public async Task Delete(int id)
        {
            var orderLine = new OrderLine { OrderLineId = id };
            _context.OrderLines.Remove(orderLine);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<OrderLine>> GetAll()
        {
            return await _context.OrderLines.AsNoTracking().ToListAsync();
        }

        public async Task<OrderLine?> GetById(int id)
        {
            return await _context.OrderLines.FindAsync(id);
        }

        public async Task Update(OrderLine orderLine)
        {
            _context.OrderLines.Update(orderLine);
            await _context.SaveChangesAsync();
        }
    }
}
