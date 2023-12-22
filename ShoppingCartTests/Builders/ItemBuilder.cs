using ShoppingCartService.DataAccess.Entities;

namespace ShoppingCartServiceTests.Builders
{
    public class ItemBuilder
    {
        private double _price;
        private uint _quantity;

        public ItemBuilder WithPrice(double price)
        {
            _price = price;
            return this;
        }

        public ItemBuilder WithQuantity(uint quantity)
        {
            _quantity = quantity;
            return this;
        }

        public Item Build()
        {
            return new Item
            {
                Price = _price,
                Quantity = _quantity
            };
        }
    }
}
