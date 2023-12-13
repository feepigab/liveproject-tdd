
using ShoppingCartService.BusinessLogic.Validation;
using ShoppingCartService.Models;

namespace UnitTests
{
    public class AddressValidatorTests
    {
        [Fact]
        public void Address_is_valid()
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
    }
}