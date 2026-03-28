using Microsoft.AspNetCore.Http;
using Webshop.Domain.Entities;

namespace Webshop.Application.Services.Contracts
{
    public interface IProductService
    {
        Task<IEnumerable<Product>> GetAll(int? categoryId = null);
        Task<Product?> GetById(int id);
        Task Add(Product product, string? providedImageUrl);
        Task Update(Product product, IFormFile? newImage, string? currentImageUrl);
        Task Delete(int id);
    }
}
