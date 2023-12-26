using MongoDB.Bson;
using MongoDB.Driver;
using ShoppingCartService.Config;
using ShoppingCartService.DataAccess;
using ShoppingCartServiceTests.Builders;

namespace ShoppingCartServiceTests.Fixtures
{
    [CollectionDefinition("MongoDB Docker")]
    public class DatabaseCollection : ICollectionFixture<DatabaseFixture> { }

    [Collection("MongoDB Docker")]
    public class ShoppingCartRepositoryIntegrationTests : IDisposable
    {
        private readonly ShoppingCartDatabaseSettings _databaseSettings;
        private readonly ShoppingCartRepository _shoppingCartRepo;
        private readonly string _id;

        public ShoppingCartRepositoryIntegrationTests(DatabaseFixture dbFixture)
        {
            _databaseSettings = dbFixture.GetDatabaseSettings();
            _shoppingCartRepo = new ShoppingCartRepository(_databaseSettings);
            var address = new AddressBuilder().WithCountry("country").WithCity("city").WithStreet("street").Build();
            var cart = new CartBuilder().WithShippingAddress(address).Build();
            _id = _shoppingCartRepo.Create(cart).Id;
        }

        [Fact]
        public void Return_All_Shared_Cart()
        {
            var repo = new ShoppingCartRepository(_databaseSettings);

            var carts = repo.FindAll();

            Assert.Single(carts);
        }

        [Fact]
        public void Insert_New_Cart_Successfully()
        {
            var address = new AddressBuilder().WithCountry("country").WithCity("city").WithStreet("street").Build();
            var cart = new CartBuilder().WithShippingAddress(address).Build();
            var newCart = _shoppingCartRepo.Create(cart);

            Assert.NotNull(newCart);
        }

        [Fact]
        public void Insert_New_Cart_With_Duplicate_Id()
        {
            var cart = new CartBuilder().WithId(_id).Build();

            Assert.Throws<MongoWriteException>(() => _shoppingCartRepo.Create(cart));
        }

        [Fact]
        public void Find_Item_With_Valid_Id()
        {
            var cart = _shoppingCartRepo.FindById(_id);

            Assert.NotNull(cart);
        }

        [Fact]
        public void Find_Item_With_Invalid_Id()
        {
            var id = ObjectId.GenerateNewId().ToString();
            var shoppingCartRepo = new ShoppingCartRepository(_databaseSettings);

            var cart = shoppingCartRepo.FindById(id);

            Assert.Null(cart);
        }

        [Fact]
        public void Update_Shipping_Address()
        {
            var newAddress = new AddressBuilder().WithCountry("country2").WithCity("city2").WithStreet("street2").Build();
            var cart = _shoppingCartRepo.FindById(_id);
            cart.ShippingAddress = newAddress;

            _shoppingCartRepo.Update(cart.Id, cart);

            Assert.Equal(newAddress.Street, _shoppingCartRepo.FindById(_id).ShippingAddress.Street);
        }

        [Fact]
        public void Remove_Shared_Cart_by_Id()
        {
            _shoppingCartRepo.Remove(_id);

            Assert.Empty(_shoppingCartRepo.FindAll());
        }

        [Fact]
        public void Remove_Shared_Cart_by_Cart_Info()
        {
            var cart = new CartBuilder().WithId(_id).Build();

            _shoppingCartRepo.Remove(cart);

            Assert.Empty(_shoppingCartRepo.FindAll());
        }

        public void Dispose()
        {
            var client = new MongoClient(_databaseSettings.ConnectionString);
            client.DropDatabase(_databaseSettings.DatabaseName);
        }
    }
}
