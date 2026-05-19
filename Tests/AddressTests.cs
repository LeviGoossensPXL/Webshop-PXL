using NUnit.Framework;
using Webshop.Domain.Entities;

namespace Tests
{
    public class AddressTests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void NewAddress_DefaultCountryIsBelgium()
        {
            // Arrange & Act
            var address = new Address();

            // Assert
            Assert.That(address.Country, Is.EqualTo("Belgium"));
        }
    }
}
