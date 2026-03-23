using Webshop.Domain.Entities;

namespace Webshop.Application.Services.Contracts
{
    public interface IProductService
    {
        Task<IEnumerable<Product>> GetAll();
        Task<Product?> GetById(int id);
        Task Add(Product product);
        Task Update(Product product);
        Task Delete(int id);
    }
}
