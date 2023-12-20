using AutoMapper;
using Moq;
using ShoppingCartService.BusinessLogic;
using ShoppingCartService.DataAccess.Entities;
using ShoppingCartService.Models;

namespace BusinessLogicTests
{
    public class CheckOutEngineTests
    {
        private readonly Mock<IShippingCalculator> _mockShippingCalculator;
        private readonly Mock<IMapper> _mockMapper;

        public CheckOutEngineTests()
        {
            _mockShippingCalculator = new Mock<IShippingCalculator>();
            _mockMapper = new Mock<IMapper>();
        }

        [InlineData(0, 2, CustomerType.Standard, 1)]
        [InlineData(0, 4, CustomerType.Standard, 3)]
        [InlineData(10, 1.8, CustomerType.Premium, 1)]
        [InlineData(10, 3.6, CustomerType.Premium, 3)]
        [Theory]
        public void Checkout_Totals(double expectedDiscount, double expectedTotal, CustomerType customerType, uint itemQuantity)
        {
            var cart = new Cart
            {
                Items = new List<Item>
                {
                    new Item
                    {
                        Price = 1.0,
                        Quantity = itemQuantity
                    }
                },
                CustomerType = customerType
            };

            var engine = new CheckOutEngine(_mockShippingCalculator.Object, _mockMapper.Object);
            _mockShippingCalculator.Setup(x => x.CalculateShippingCost(cart)).Returns(1.0);

            var result = engine.CalculateTotals(cart);

            Assert.Equal(expectedDiscount, result.CustomerDiscount);
            Assert.Equal(expectedTotal, result.Total);
        }
    }
}
