using NUnit.Framework;
using Webshop.Domain.Entities;
using Webshop.Application.Services;
using Webshop.Application.Repositories;
using Moq;
using System.Threading.Tasks;

namespace Tests
{
    public class ProductServiceTests
    {
        [Test]
        public async Task AddProduct_WithEmptyImageUrl_AssignsDefaultImage()
        {
            // Arrange
            var mockRepo = new Mock<IProductRepository>();
            var service = new ProductService(mockRepo.Object);
            var product = new Product { Name = "Test Product" };

            // Act
            await service.Add(product, string.Empty);

            // Assert
            Assert.That(product.ImageUrl, Is.EqualTo("/images/default.jpg"));
            mockRepo.Verify(r => r.Add(product), Times.Once); // Verifies product is saved
        }

        [Test]
        public async Task AddProduct_WithProvidedImageUrl_UsesProvidedImage()
        {
            // Arrange
            var mockRepo = new Mock<IProductRepository>();
            var service = new ProductService(mockRepo.Object);
            var product = new Product { Name = "Test Product" };
            var imageUrl = "/images/custom.jpg";

            // Act
            await service.Add(product, imageUrl);

            // Assert
            Assert.That(product.ImageUrl, Is.EqualTo(imageUrl));
            mockRepo.Verify(r => r.Add(product), Times.Once);
        }
    }
}
