using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Webshop.Application.Repositories;
using Webshop.Domain.Entities;

namespace Webshop.Application.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;

        public ProductService(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public async Task<IEnumerable<Product>> GetAll()
        {
            return await _productRepository.GetAll();
        }

        public async Task<Product?> GetById(int id)
        {
            return await _productRepository.GetById(id);
        }

        public async Task Add(Product product)
        {
            await _productRepository.Add(product);
        }

        public async Task Update(Product product)
        {
            await _productRepository.Update(product);
        }

        public async Task Delete(int id)
        {
            await _productRepository.Delete(id);
        }
    }
}
