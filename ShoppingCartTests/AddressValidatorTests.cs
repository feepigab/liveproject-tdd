
using ShoppingCartService.BusinessLogic.Validation;
using ShoppingCartService.Models;

namespace BusinessLogicTests
{
    public class AddressValidatorTests
    {
        [Fact]
        public void Valid_address()
        {
            // Arrange
            var address = new Address
            {
                Country = "USA",
                City = "Phoenix",
                Street = "123 Main Ave"
            };

            var addressValidator = new AddressValidator();

            // Act
            bool isValid = addressValidator.IsValid(address);

            // Assert
            Assert.True(isValid);
        }

        [Fact]
        public void Invalid_address_no_country()
        {
            var address = new Address
            {
                Country = string.Empty,
                City = "Phoenix",
                Street = "123 Main Ave"
            };
            var addressValidator = new AddressValidator();

            bool isValid = addressValidator.IsValid(address);

            Assert.False(isValid);
        }

        [Fact]
        public void Invalid_address_no_city()
        {
            var address = new Address
            {
                Country = "USA",
                City = string.Empty,
                Street = "123 Main Ave"
            };
            var addressValidator = new AddressValidator();

            bool isValid = addressValidator.IsValid(address);

            Assert.False(isValid);
        }

        [Fact]
        public void Invalid_address_no_street()
        {
            var address = new Address
            {
                Country = "USA",
                City = "Phoenix",
                Street = string.Empty
            };
            var addressValidator = new AddressValidator();

            bool isValid = addressValidator.IsValid(address);

            Assert.False(isValid);
        }

        [Fact]
        public void Invalid_address_null()
        {
            var addressValidator = new AddressValidator();

            bool isValid = addressValidator.IsValid(null);

            Assert.False(isValid);
        }
    }
}