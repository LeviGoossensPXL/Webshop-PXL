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

        public async Task<ServiceResultOfT<IEnumerable<Product>>> GetProducts(int? categoryId = null)
        {
            var result = new ServiceResultOfT<IEnumerable<Product>>();
            var products = await _productRepository.GetAll();

            var sorted = products
                .OrderBy(p => p.CategoryId)
                .ThenByDescending(p => p.ProductId)
                .AsEnumerable();

            if (categoryId.HasValue && categoryId.Value > 0)
                sorted = sorted.Where(p => p.CategoryId == categoryId.Value);

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
