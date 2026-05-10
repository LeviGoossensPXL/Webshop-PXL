using Webshop.Application.Repositories;
using Webshop.Application.Results;
using Webshop.Application.Services.Contracts;
using Webshop.Domain.Entities;

namespace Webshop.Application.Services
{
    public class ShopService : IShopService
    {
        private readonly IProductRepository _productRepository;
        private readonly ICategoryRepository _categoryRepository;

        public ShopService(IProductRepository productRepository, ICategoryRepository categoryRepository)
        {
            _productRepository = productRepository;
            _categoryRepository = categoryRepository;
        }

        public async Task<ServiceResultOfT<IEnumerable<Product>>> GetProducts(int? categoryId = null, string? searchQuery = null)
        {
            var result = new ServiceResultOfT<IEnumerable<Product>>();
            var products = await _productRepository.GetAll();

            if (categoryId.HasValue && categoryId.Value > 0)
                products = products.Where(p => p.CategoryId == categoryId.Value);

            if (!string.IsNullOrWhiteSpace(searchQuery))
            {
                var query = searchQuery.Trim().ToLower();
                products = products.Where(p => 
                    (p.Name != null && p.Name.ToLower().Contains(query)));
            }

            var sorted = products
                .OrderBy(p => p.CategoryId)
                .ThenByDescending(p => p.ProductId)
                .AsEnumerable();

            result.Data = sorted;
            return result;
        }

        public async Task<ServiceResultOfT<Product>> GetProductById(int id)
        {
            var result = new ServiceResultOfT<Product>();
            var product = await _productRepository.GetById(id);

            if (product == null)
            {
                result.Failed("Product not found.");
                return result;
            }

            result.Data = product;
            return result;
        }

        public async Task<IEnumerable<Category>> GetCategories()
        {
            return await _categoryRepository.GetAll();
        }
    }
}
