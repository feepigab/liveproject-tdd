using ShoppingCartService.BusinessLogic.Validation;
using ShoppingCartTests.Builders;

namespace BusinessLogicTests
{
    public class AddressValidatorTests
    {
        [Fact]
        public void Valid_address()
        {
            var address = new AddressBuilder().WithCountry("Country").WithCity("City").WithStreet("123 Street Name").Build();

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
            var address = new AddressBuilder().WithCountry(country).WithCity(city).WithStreet(street).Build();

            var validator = new AddressValidator();
            bool isValid = validator.IsValid(address);

            Assert.False(isValid);
        }
    }
}