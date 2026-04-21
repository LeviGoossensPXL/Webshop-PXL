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

        public async Task<IEnumerable<Category>> GetAllCategoriesAsync()
        {
            return await _categoryRepository.GetAll();
        }

        public async Task<Category?> GetCategoryByIdAsync(int id)
        {
            return await _categoryRepository.GetById(id);
        }

        public async Task AddCategoryAsync(Category category)
        {
            await _categoryRepository.Add(category);
        }

        public async Task UpdateCategoryAsync(Category category)
        {
            await _categoryRepository.Update(category);
        }

        public async Task DeleteCategoryAsync(int id)
        {
            await _categoryRepository.Delete(id);
        }
    }
}
