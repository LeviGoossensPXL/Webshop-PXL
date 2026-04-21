using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Webshop.Application.Repositories;
using Webshop.Application.Services.Contracts;
using Webshop.Domain.Entities;

namespace Webshop.Application.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _categoryRepository;

        public CategoryService(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        public async Task<IEnumerable<Category>> GetAll()
        {
            return await _categoryRepository.GetAll();
        }

        public async Task<Category?> GetById(int id)
        {
            return await _categoryRepository.GetById(id);
        }

        public async Task Add(Category category)
        {
            await _categoryRepository.Add(category);
        }

        public async Task Update(Category category)
        {
            await _categoryRepository.Update(category);
        }

        public async Task Delete(int id)
        {
            await _categoryRepository.Delete(id);
        }
    }
}
