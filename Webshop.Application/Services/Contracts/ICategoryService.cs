using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Webshop.Domain.Entities;

namespace Webshop.Application.Services.Contracts
{
    public interface ICategoryService
    {
        Task<IEnumerable<Category>> GetAll();
        Task<Category?> GetById(int id);
        Task Add(Category category);
        Task Update(Category category);
        Task Delete(int id);
    }
}
