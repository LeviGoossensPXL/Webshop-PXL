using NUnit.Framework;
using Webshop.Domain.Entities;
using System;

namespace Tests
{
    public class OrderTests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void NewOrder_HasPendingStatus()
        {
            // Arrange
            var order = new Order();

            // Act
            var status = order.Status;

            // Assert
            Assert.That(status, Is.EqualTo(OrderStatus.Pending));
        }

        [Test]
        public void NewOrder_HasCurrentDate()
        {
            // Arrange & Act
            var order = new Order();

            // Assert
            // Just check if it's recent so the test doesn't randomly fail
            var timeDifference = DateTime.UtcNow - order.OrderDate;
            Assert.That(timeDifference.TotalMinutes, Is.LessThan(1));
        }
    }
}
