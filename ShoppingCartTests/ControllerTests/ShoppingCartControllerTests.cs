using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MongoDB.Bson;
using Moq;
using ShoppingCartService.BusinessLogic;
using ShoppingCartService.BusinessLogic.Exceptions;
using ShoppingCartService.BusinessLogic.Validation;
using ShoppingCartService.Controllers;
using ShoppingCartService.Controllers.Models;
using ShoppingCartService.DataAccess;
using ShoppingCartService.DataAccess.Entities;
using ShoppingCartService.Mapping;
using ShoppingCartService.Models;
using ShoppingCartServiceTests.Builders;

namespace ShoppingCartServiceTests.ControllerTests
{
    public class ShoppingCartControllerTests
    {
        private readonly Mock<ILogger<ShoppingCartController>> _loggerMock;
        private readonly Mock<IShoppingCartRepository> _shoppingCartRepoMock;
        private readonly Mock<IAddressValidator> _addressValidatorMock;
        private readonly Mock<ICheckOutEngine> _checkoutEngineMock;
        private readonly IMapper _mapper;
        private readonly ShoppingCartManager _shoppingCartManager;


        public ShoppingCartControllerTests()
        {
            _loggerMock = new Mock<ILogger<ShoppingCartController>>();
            _shoppingCartRepoMock = new Mock<IShoppingCartRepository>();
            _addressValidatorMock = new Mock<IAddressValidator>();
            _checkoutEngineMock = new Mock<ICheckOutEngine>();
            _mapper = new Mapper(new MapperConfiguration(cfg => cfg.AddProfile(new MappingProfile())));
            _shoppingCartManager = new ShoppingCartManager(
                _shoppingCartRepoMock.Object,
                _addressValidatorMock.Object,
                _mapper,
                _checkoutEngineMock.Object);
        }

        [Fact]
        public void Get_All_Returns_One_Item()
        {
            var shoppingCartController = new ShoppingCartController(_shoppingCartManager, _loggerMock.Object);

            _shoppingCartRepoMock.Setup(x => x.FindAll()).Returns(new List<Cart> { new Cart() });

            var carts = shoppingCartController.GetAll();

            Assert.Single(carts);
        }

        [Fact]
        public void Get_By_Id_Not_Found()
        {
            var shoppingCartController = new ShoppingCartController(_shoppingCartManager, _loggerMock.Object);

            var id = ObjectId.GenerateNewId().ToString();

            _shoppingCartRepoMock.Setup(x => x.FindById(id)).Returns((Cart)null);

            var cart = shoppingCartController.FindById(id);

            Assert.IsType<NotFoundResult>(cart.Result);
        }

        [Fact]
        public void Get_By_Id_Returns_Correct_Cart()
        {
            var shoppingCartController = new ShoppingCartController(_shoppingCartManager, _loggerMock.Object);

            var id = ObjectId.GenerateNewId().ToString();

            _shoppingCartRepoMock.Setup(x => x.FindById(id)).Returns(new CartBuilder().WithId(id).Build());

            var cart = shoppingCartController.FindById(id).Value;

            Assert.Equal(id, cart.Id);
        }

        [Fact]
        public void Calculate_Totals_Throws_CartNotFound_Exception()
        {
            var shoppingCartController = new ShoppingCartController(_shoppingCartManager, _loggerMock.Object);

            var id = ObjectId.GenerateNewId().ToString();

            _shoppingCartRepoMock.Setup(x => x.FindById(id)).Returns((Cart)null);

            var totals = shoppingCartController.CalculateTotals(id);

            Assert.Throws<ShoppingCartNotFoundException>(() => _shoppingCartManager.CalculateTotals(id));
            Assert.IsType<NotFoundResult>(totals.Result);
        }

        [Fact]
        public void Calculate_Totals_Returns_Valid_CheckoutDto()
        {
            var id = ObjectId.GenerateNewId().ToString();
            var cart = new CartBuilder().WithId(id).Build();
            var cartDto = _mapper.Map<ShoppingCartDto>(cart);
            var checkoutDto = new CheckoutDto(cartDto, 0, 0, 0);
            var shoppingCartController = new ShoppingCartController(_shoppingCartManager, _loggerMock.Object);

            _shoppingCartRepoMock.Setup(x => x.FindById(id)).Returns(cart);
            _checkoutEngineMock.Setup(x => x.CalculateTotals(cart)).Returns(checkoutDto);

            var totals = shoppingCartController.CalculateTotals(id);

            Assert.Equal(totals.Value.ShoppingCart.Id, id);
        }

        [Fact]
        public void Create_Successful_Matching_Ids()
        {
            var id = ObjectId.GenerateNewId().ToString();
            var item = new ItemBuilder().WithQuantity(10).Build();
            var cartDto = new CreateCartDto
            {
                Customer = new CustomerDto { Address = new Address() },
                Items = new List<ItemDto> { _mapper.Map<ItemDto>(item) }
            };

            _addressValidatorMock.Setup(x => x.IsValid(cartDto.Customer.Address)).Returns(true);
            // TODO: mock repo to return cart with id

            var shoppingCartController = new ShoppingCartController(_shoppingCartManager, _loggerMock.Object);

            var result = shoppingCartController.Create(cartDto);

            Assert.Equal(id, result.Value.Id);
        }

        [Fact]
        public void Create_Invalid_Address_Throws_InvalidInputException()
        {
            var cartDto = new CreateCartDto
            {
                Customer = new CustomerDto { Address = new Address() }
            };

            _addressValidatorMock.Setup(x => x.IsValid(cartDto.Customer.Address)).Returns(false);
            var shoppingCartController = new ShoppingCartController(_shoppingCartManager, _loggerMock.Object);

            var result = shoppingCartController.Create(cartDto);

            Assert.Throws<InvalidInputException>(() => _shoppingCartManager.Create(cartDto));
            Assert.IsType<BadRequestResult>(result.Result);
        }

        [Fact]
        public void Delete_Successul()
        {
        }
    }
}
