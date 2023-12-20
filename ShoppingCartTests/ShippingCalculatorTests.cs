using ShoppingCartService.BusinessLogic;
using ShoppingCartService.DataAccess.Entities;
using ShoppingCartService.Models;
using ShoppingCartTests.Builders;

namespace BusinessLogicTests
{
    public class ShippingCalculatorTests
    {
        [InlineData(1.0, "USA", "Dallas")]
        [InlineData(2.0, "USA", "City")]
        [InlineData(15.0, "Country", "City")]
        [Theory]
        public void Shipping_Cost_by_Location_Standard(double expected, string country, string city)
        {
            var cart = GenerateCart(country, city);

            var shippingCalculator = new ShippingCalculator();
            var cost = shippingCalculator.CalculateShippingCost(cart);

            Assert.Equal(expected, cost);
        }

        [InlineData(1.0, ShippingMethod.Standard)]
        [InlineData(1.2, ShippingMethod.Expedited)]
        [InlineData(2.0, ShippingMethod.Priority)]
        [InlineData(2.5, ShippingMethod.Express)]
        [Theory]
        public void Shipping_Cost_by_Method(double expected, ShippingMethod shippingMethod)
        {
            var cart = GenerateCart(shippingMethod: shippingMethod);

            var shippingCalculator = new ShippingCalculator();
            var cost = shippingCalculator.CalculateShippingCost(cart);

            Assert.Equal(expected, cost);
        }

        [InlineData(2.0, CustomerType.Standard)]
        [InlineData(1.0, CustomerType.Premium)]
        [Theory]
        public void Shipping_Cost_by_Customer_Type_Priority_Method(double expected, CustomerType customerType)
        {
            var cart = GenerateCart(customerType: customerType, shippingMethod: ShippingMethod.Priority);

            var shippingCalculator = new ShippingCalculator();
            var cost = shippingCalculator.CalculateShippingCost(cart);

            Assert.Equal(expected, cost);
        }

        [Fact]
        public void Foreign_Office()
        {
            var officeAddress = new AddressBuilder().WithCountry("Canada").Build();

            var cart = GenerateCart(country: "USA");

            var shippingCalculator = new ShippingCalculator(officeAddress);
            var cost = shippingCalculator.CalculateShippingCost(cart);

            Assert.Equal(15.0, cost);
        }

        private Cart GenerateCart(
            string country = "USA",
            string city = "Dallas",
            CustomerType customerType = CustomerType.Standard,
            ShippingMethod shippingMethod = ShippingMethod.Standard)
        {
            return new CartBuilder()
                .WithCustomerType(customerType)
                .WithShippingMethod(shippingMethod)
                .WithShippingAddress(new AddressBuilder().WithCountry(country).WithCity(city).WithStreet("123 Street Name").Build())
                .WithItems(new List<Item> { new ItemBuilder().WithQuantity(1).Build() })
                .Build();
        }
    }
}
