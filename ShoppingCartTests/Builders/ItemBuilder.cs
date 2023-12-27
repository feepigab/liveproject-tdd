using ShoppingCartService.DataAccess.Entities;

namespace ShoppingCartServiceTests.Builders
{
    public class ItemBuilder
    {
        private string _productId;
        private double _price;
        private uint _quantity;

        public ItemBuilder()
        {
            _productId = "default";
        }

        public ItemBuilder WithProductId(string productId)
        {
            _productId = productId;
            return this;
        }
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
                ProductId = _productId,
                Price = _price,
                Quantity = _quantity
            };
        }
    }
}
