using ShoppingCartService.BusinessLogic;
using ShoppingCartService.DataAccess.Entities;
using ShoppingCartService.Models;

namespace BusinessLogicTests
{
    public class ShippingCalculatorTests
    {
        [Fact]
        public void Same_city_standard()
        {
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

            var shippingCalculator = new ShippingCalculator();
            var cost = shippingCalculator.CalculateShippingCost(cart);

            Assert.Equal(1.0, cost);
        }

        [Fact]
        public void Same_country_standard()
        {
            var cart = new Cart
            {
                CustomerType = CustomerType.Standard,
                ShippingMethod = ShippingMethod.Standard,
                ShippingAddress = new Address
                {
                    Country = "USA",
                    City = "Phoenix",
                    Street = "123 Main Ave"
                },
                Items = new List<Item>
                {
                    new Item{Quantity=1}
                }
            };

            var shippingCalculator = new ShippingCalculator();
            var cost = shippingCalculator.CalculateShippingCost(cart);

            Assert.Equal(2.0, cost);
        }

        [Fact]
        public void International_standard()
        {
            var cart = new Cart
            {
                CustomerType = CustomerType.Standard,
                ShippingMethod = ShippingMethod.Standard,
                ShippingAddress = new Address
                {
                    Country = "Canada",
                    City = "Toronto",
                    Street = "123 Main Ave"
                },
                Items = new List<Item>
                {
                    new Item{Quantity=1}
                }
            };

            var shippingCalculator = new ShippingCalculator();
            var cost = shippingCalculator.CalculateShippingCost(cart);

            Assert.Equal(15.0, cost);
        }

        [Fact]
        public void Expedited_shippping()
        {
            var cart = new Cart
            {
                CustomerType = CustomerType.Standard,
                ShippingMethod = ShippingMethod.Expedited,
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

            var shippingCalculator = new ShippingCalculator();
            var cost = shippingCalculator.CalculateShippingCost(cart);

            Assert.Equal(1.2, cost);
        }

        [Fact]
        public void Priority_shippping()
        {
            var cart = new Cart
            {
                CustomerType = CustomerType.Standard,
                ShippingMethod = ShippingMethod.Priority,
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

            var shippingCalculator = new ShippingCalculator();
            var cost = shippingCalculator.CalculateShippingCost(cart);

            Assert.Equal(2.0, cost);
        }

        [Fact]
        public void Express_shippping()
        {
            var cart = new Cart
            {
                CustomerType = CustomerType.Standard,
                ShippingMethod = ShippingMethod.Express,
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

            var shippingCalculator = new ShippingCalculator();
            var cost = shippingCalculator.CalculateShippingCost(cart);

            Assert.Equal(2.5, cost);
        }

        [Fact]
        public void Premium_Customer()
        {
            var cart = new Cart
            {
                CustomerType = CustomerType.Premium,
                ShippingMethod = ShippingMethod.Priority,
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

            var shippingCalculator = new ShippingCalculator();
            var cost = shippingCalculator.CalculateShippingCost(cart);

            Assert.Equal(1.0, cost);
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
    }
}
