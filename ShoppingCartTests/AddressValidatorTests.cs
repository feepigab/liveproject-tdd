
using ShoppingCartService.BusinessLogic.Validation;
using ShoppingCartService.Models;

namespace BusinessLogicTests
{
    public class AddressValidatorTests
    {
        [Fact]
        public void Valid_address()
        {
            var address = new Address
            {
                Country = "USA",
                City = "Phoenix",
                Street = "123 Main Ave"
            };
            var addressValidator = new AddressValidator();

            bool isValid = addressValidator.IsValid(address);

            Assert.True(isValid);
        }

        [InlineData("", "City", "Street")]
        [InlineData("Country", "", "Street")]
        [InlineData("Country", "City", "")]
        [InlineData(null, null, null)]
        [Theory]
        public void Address_is_Invalid(string country, string city, string street)
        {
            var address = GenerateAddress(country, city, street);

            var validator = new AddressValidator();
            bool isValid = validator.IsValid(address);

            Assert.False(isValid);
        }

        private Address GenerateAddress(string country, string city, string street)
        {
            if (country == null || city == null || street == null) { return null; }

            return new Address
            {
                Country = country,
                City = city,
                Street = street
            };
        }
    }
}