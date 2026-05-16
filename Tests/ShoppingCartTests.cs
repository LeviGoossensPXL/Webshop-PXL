using NUnit.Framework;
using Webshop.Domain.Entities;
using System.Collections.Generic;

namespace Tests
{
    public class ShoppingCartTests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void CalculateTotalPrice_ReturnsCorrectAmount()
        {
            // Arrange
            var cart = new ShoppingCart();
            cart.Items.Add(new ShoppingCartItem { ProductId = 1, Quantity = 2, UnitPrice = 10.50m });
            cart.Items.Add(new ShoppingCartItem { ProductId = 2, Quantity = 1, UnitPrice = 5.00m });

            // Act
            var total = cart.TotalPrice;

            // Assert
            Assert.That(total, Is.EqualTo(26.00m));
        }

        [Test]
        public void EmptyCart_TotalPriceIsZero()
        {
            // Arrange
            var cart = new ShoppingCart();

            // Act
            var total = cart.TotalPrice;

            // Assert
            Assert.That(total, Is.EqualTo(0m));
        }
    }
}