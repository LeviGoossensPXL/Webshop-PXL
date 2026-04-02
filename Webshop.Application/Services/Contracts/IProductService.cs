using Microsoft.AspNetCore.Http;
using Webshop.Application.Results;
using Webshop.Domain.Entities;

namespace Webshop.Application.Services.Contracts
{
    public interface IProductService
    {
        Task<ServiceResultOfT<IEnumerable<Product>>> GetAll(int? categoryId = null);
        Task<ServiceResultOfT<Product>> GetById(int id);
        Task<ServiceResult> Add(Product product, string? providedImageUrl);
        Task<ServiceResult> Update(Product product, IFormFile? newImage, string? currentImageUrl);
        Task<ServiceResult> Delete(int id);
    }
}
