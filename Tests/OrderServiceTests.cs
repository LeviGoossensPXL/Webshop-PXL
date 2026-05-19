using NUnit.Framework;
using Webshop.Domain.Entities;
using Webshop.Application.Services;
using Webshop.Application.Repositories;
using Moq;
using Moq.Protected;
using System.Net.Http;
using System.Threading.Tasks;
using System.Threading;
using System.Net;
using System.Collections.Generic;

namespace Tests
{
    public class OrderServiceTests
    {
        [Test]
        public async Task UpdateOrderStatus_FromPendingToDelivered_UpdatesStatusWithoutStockApi()
        {
            // Arrange
            var orderId = 1;
            var order = new Order { OrderId = orderId, Status = OrderStatus.Pending };
            
            var mockRepo = new Mock<IOrderRepository>();
            mockRepo.Setup(r => r.GetById(orderId)).ReturnsAsync(order);

            // StockAPI shouldn't be called for "Pending" -> "Delivered", so we don't mock HttpClient behavior
            var mockHttpFactory = new Mock<IHttpClientFactory>();

            var service = new OrderService(mockRepo.Object, mockHttpFactory.Object);

            // Act
            var result = await service.UpdateOrderStatus(orderId, (int)OrderStatus.Delivered);

            // Assert
            Assert.That(result.Succeeded, Is.True);
            Assert.That(order.Status, Is.EqualTo(OrderStatus.Delivered));
            mockRepo.Verify(r => r.Update(order), Times.Once);
            mockHttpFactory.Verify(f => f.CreateClient(It.IsAny<string>()), Times.Never); // Ensures StockAPI is untouched
        }

        [Test]
        public async Task UpdateOrderStatus_FromPendingToCancelled_CallsStockApiToRestore()
        {
            // Arrange
            var orderId = 2;
            var order = new Order 
            { 
                OrderId = orderId, 
                Status = OrderStatus.Pending,
                OrderLines = new List<OrderLine> 
                { 
                    new OrderLine { ProductId = 10, Quantity = 2 } 
                }
            };

            var mockRepo = new Mock<IOrderRepository>();
            mockRepo.Setup(r => r.GetById(orderId)).ReturnsAsync(order);

            // To test Cancelled status, we must simulate a successful StockAPI response using Moq.Protected
            var mockMessageHandler = new Mock<HttpMessageHandler>();
            mockMessageHandler.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>()
                )
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK
                });

            var httpClient = new HttpClient(mockMessageHandler.Object) { BaseAddress = new System.Uri("http://localhost") };

            var mockHttpFactory = new Mock<IHttpClientFactory>();
            mockHttpFactory.Setup(f => f.CreateClient("StockApi")).Returns(httpClient);

            var service = new OrderService(mockRepo.Object, mockHttpFactory.Object);

            // Act
            var result = await service.UpdateOrderStatus(orderId, (int)OrderStatus.Cancelled);

            // Assert
            Assert.That(result.Succeeded, Is.True);
            Assert.That(order.Status, Is.EqualTo(OrderStatus.Cancelled));
            mockRepo.Verify(r => r.Update(order), Times.Once);
            mockHttpFactory.Verify(f => f.CreateClient("StockApi"), Times.Once); // StockAPI must be called
        }
    }
}
