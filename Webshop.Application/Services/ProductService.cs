using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Webshop.Application.Repositories;
using Webshop.Application.Results;
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

        public async Task<ServiceResultOfT<IEnumerable<Product>>> GetAll(int? categoryId = null)
        {
            var result = new ServiceResultOfT<IEnumerable<Product>>();
            var products = await _productRepository.GetAll();

            // filter products by category if a valid category ID is provided
            if (categoryId.HasValue && categoryId.Value > 0)
            {
                products = products.Where(p => p.CategoryId == categoryId.Value);
            }

            result.Data = products;
            return result;
        }

        public async Task<ServiceResultOfT<Product>> GetById(int id)
        {
            var result = new ServiceResultOfT<Product>();
            var product = await _productRepository.GetById(id);

            // return a failed result instead of null if the entity does not exist
            if (product == null)
            {
                result.Failed("Product not found.");
                return result;
            }

            result.Data = product;
            return result;
        }

        public async Task<ServiceResult> Add(Product product, string? providedImageUrl)
        {
            var result = new ServiceResult();

            // assign default image if no image URL is provided by the user
            product.ImageUrl = string.IsNullOrWhiteSpace(providedImageUrl)
                ? "/images/default.jpg"
                : providedImageUrl;

            await _productRepository.Add(product);
            return result;
        }

        public async Task<ServiceResult> Update(Product product, IFormFile? newImage, string? currentImageUrl)
        {
            var result = new ServiceResult();
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
            return result;
        }

        public async Task<ServiceResult> Delete(int id)
        {
            var result = new ServiceResult();
            var product = await _productRepository.GetById(id);
            if (product == null)
            {
                result.Failed("Product not found and could not be deleted.");
                return result;
            }
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
            return result;
        }
    }
}
