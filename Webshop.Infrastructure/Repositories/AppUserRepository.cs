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
    public class AppUserRepository : IAppUserRepository
    {
        private readonly AppDbContext _context;
        public AppUserRepository(AppDbContext context)
        {
            _context = context;
        }
        public async Task Delete(string id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user != null)
            {
                _context.Users.Remove(user);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<AppUser>> GetAll()
        {
            return await _context.Users.ToListAsync();
        }

        public async  Task<AppUser?> GetById(string id)
        {
           return await _context.Users.FindAsync(id);
        }
    }
}
