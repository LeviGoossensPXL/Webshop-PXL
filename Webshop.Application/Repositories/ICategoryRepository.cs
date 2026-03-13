using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Webshop.Domain.Entities;

namespace Webshop.Application.Repositories
{
    public interface ICategoryRepository
    {
        Task<IEnumerable<Category>> GetAll();
        Task<Category?> GetById(int id);

        // new methods for adding, updating, and deleting categories
        Task Add(Category category); 
        Task Update(Category category); 
        Task Delete(int id); 
    }
}
