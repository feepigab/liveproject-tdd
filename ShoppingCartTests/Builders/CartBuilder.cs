using ShoppingCartService.DataAccess.Entities;
using ShoppingCartService.Models;

namespace ShoppingCartServiceTests.Builders
{
    public class CartBuilder
    {
        private CustomerType _customerType;
        private ShippingMethod _shippingMethod;
        private Address _shippingAddress;
        private List<Item> _items;

        public CartBuilder()
        {
            _shippingAddress = new AddressBuilder().Build();
            _items = new List<Item>();
        }

        public CartBuilder WithCustomerType(CustomerType customerType)
        {
            _customerType = customerType;
            return this;
        }

        public CartBuilder WithShippingMethod(ShippingMethod shippingMethod)
        {
            _shippingMethod = shippingMethod;
            return this;
        }

        public CartBuilder WithShippingAddress(Address shippingAddress)
        {
            _shippingAddress = shippingAddress;
            return this;
        }

        public CartBuilder WithItems(List<Item> items)
        {
            _items = items;
            return this;
        }

        public Cart Build()
        {
            return new Cart
            {
                CustomerType = _customerType,
                ShippingMethod = _shippingMethod,
                ShippingAddress = _shippingAddress,
                Items = _items
            };
        }
    }
}
