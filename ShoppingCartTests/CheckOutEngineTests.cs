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

        [Fact]
        public void Few_items_no_discount()
        {
            var cart = new Cart
            {
                Items = new List<Item> {
                    new Item {
                        Price = 1.0,
                        Quantity = 3
                    }
                },
                CustomerType = CustomerType.Standard
            };

            var engine = new CheckOutEngine(_mockShippingCalculator.Object, _mockMapper.Object);
            _mockShippingCalculator.Setup(x => x.CalculateShippingCost(cart)).Returns(1.0);
            var result = engine.CalculateTotals(cart);

            Assert.Equal(0, result.CustomerDiscount);
            Assert.Equal(4, result.Total);
        }

        [Fact]
        public void One_item_premium_discount()
        {
            var cart = new Cart
            {
                Items = new List<Item> {
                    new Item {
                        Price = 1.0,
                        Quantity = 1
                    }
                },
                CustomerType = CustomerType.Premium
            };

            var engine = new CheckOutEngine(_mockShippingCalculator.Object, _mockMapper.Object);
            _mockShippingCalculator.Setup(x => x.CalculateShippingCost(cart)).Returns(1.0);
            var result = engine.CalculateTotals(cart);

            Assert.Equal(10.0, result.CustomerDiscount);
            Assert.Equal(1.8, result.Total); // 10% discount
        }
    }
}
