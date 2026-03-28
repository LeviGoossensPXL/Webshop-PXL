using Microsoft.AspNetCore.Http;
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
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;

        public ProductService(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public async Task<IEnumerable<Product>> GetAll(int? categoryId = null)
        {
            var products = await _productRepository.GetAll();

            // filter products by category if a valid category ID is provided
            if (categoryId.HasValue && categoryId.Value > 0)
            {
                products = products.Where(p => p.CategoryId == categoryId.Value);
            }

            return products;
        }

        public async Task<Product?> GetById(int id)
        {
            return await _productRepository.GetById(id);
        }

        public async Task Add(Product product, string? providedImageUrl)
        {
            // assign default image if no image URL is provided by the user
            product.ImageUrl = string.IsNullOrWhiteSpace(providedImageUrl)
                ? "/images/default.jpg"
                : providedImageUrl;

            await _productRepository.Add(product);
        }

        public async Task Update(Product product, IFormFile? newImage, string? currentImageUrl)
        {
            string imageUrl = currentImageUrl ?? "/images/default.jpg";

            // save the newly uploaded image to the physical file system and generate a unique filename
            if (newImage != null && newImage.Length > 0)
            {
                string fileName = Guid.NewGuid().ToString() + Path.GetExtension(newImage.FileName);
                string filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images", fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await newImage.CopyToAsync(stream);
                }

                imageUrl = "/images/" + fileName;
            }

            product.ImageUrl = imageUrl;
            await _productRepository.Update(product);
        }

        public async Task Delete(int id)
        {
            var product = await _productRepository.GetById(id);
            if (product != null)
            {
                // physically delete the image file associated with the product, skipping the default image
                if (!string.IsNullOrEmpty(product.ImageUrl) && product.ImageUrl != "/images/default.jpg")
                {
                    var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", product.ImageUrl.TrimStart('/'));
                    if (File.Exists(filePath))
                    {
                        File.Delete(filePath);
                    }
                }

                await _productRepository.Delete(id);
            }
        }
    }
}
