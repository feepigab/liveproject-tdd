using AutoMapper;
using Moq;
using ShoppingCartService.BusinessLogic;
using ShoppingCartService.DataAccess.Entities;
using ShoppingCartService.Models;
using ShoppingCartServiceTests.Builders;

namespace ShoppingCartServiceTests.BusinessLogicTests
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
            var item = new ItemBuilder().WithPrice(1.0).WithQuantity(itemQuantity).Build();
            var cart = new CartBuilder().WithItems(new List<Item> { item }).WithCustomerType(customerType).Build();

            var engine = new CheckOutEngine(_mockShippingCalculator.Object, _mockMapper.Object);
            _mockShippingCalculator.Setup(x => x.CalculateShippingCost(cart)).Returns(1.0);

            var result = engine.CalculateTotals(cart);

            Assert.Equal(expectedDiscount, result.CustomerDiscount);
            Assert.Equal(expectedTotal, result.Total);
        }
    }
}
