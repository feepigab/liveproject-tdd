using ShoppingCartService.BusinessLogic;
using ShoppingCartService.DataAccess.Entities;
using ShoppingCartService.Models;

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
            var officeAddress = new Address
            {
                Country = "Canada",
                City = "Toronto",
                Street = "123 East Ave"

            };
            var cart = new Cart
            {
                CustomerType = CustomerType.Standard,
                ShippingMethod = ShippingMethod.Standard,
                ShippingAddress = new Address
                {
                    Country = "USA",
                    City = "Dallas",
                    Street = "123 Main Ave"
                },
                Items = new List<Item>
                {
                    new Item{Quantity=1}
                }
            };

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
            return new Cart
            {
                CustomerType = customerType,
                ShippingMethod = shippingMethod,
                ShippingAddress = new Address
                {
                    Country = country,
                    City = city,
                    Street = "123 street name"
                },
                Items = new List<Item> { new Item { Quantity = 1 } }
            };
        }
    }
}
