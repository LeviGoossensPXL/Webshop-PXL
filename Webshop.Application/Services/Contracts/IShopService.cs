using Webshop.Application.Results;
using Webshop.Domain.Entities;

namespace Webshop.Application.Services.Contracts
{
    public interface IShopService
    {
        Task<ServiceResultOfT<IEnumerable<Product>>> GetProducts(int? categoryId = null, string? searchQuery = null);
        Task<ServiceResultOfT<Product>> GetProductById(int id);
        Task<IEnumerable<Category>> GetCategories();
    }
}
