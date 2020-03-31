using System;
using OmniKassa.Model.Enums;
using OmniKassa.Model.Order;
using Xunit;

namespace OmniKassa.Tests.Model.Order
{
    public class OrderItemTest
    {
        private readonly OrderItem orderItem = OrderItemFactory.OrderItem();
        private readonly OrderItem orderItemFull = OrderItemFactory.OrderItemFull();
        private readonly OrderItem orderItemNegative = OrderItemFactory.OrderItemNegative();

        [Fact]
        public void TestFields()
        {
            AssertRequiredFields(orderItem);
        }

        [Fact]
        public void TestFieldsFull()
        {
            AssertRequiredFields(orderItemFull);
            AssertOptionalFields(orderItemFull);
        }

        [Fact]
        public void TestEquals()
        {
            OrderItem orderItem1 = OrderItemFactory.OrderItem();
            OrderItem orderItem2 = OrderItemFactory.OrderItem();

            Assert.True(orderItem1.Equals(orderItem2));
            Assert.True(orderItem1.GetHashCode() == orderItem2.GetHashCode());

            OrderItem orderItem3 = OrderItemFactory.OrderItemFull();
            OrderItem orderItem4 = OrderItemFactory.OrderItemFull();

            Assert.True(orderItem3.Equals(orderItem4));
            Assert.True(orderItem3.GetHashCode() == orderItem4.GetHashCode());
        }

        [Fact]
        public void AsJson()
        {
            OrderItem expected = TestHelper.GetObjectFromJsonFile<OrderItem>("order_item.json");
            Assert.Equal(expected, orderItem);
        }

        [Fact]
        public void AsJsonFull()
        {
            OrderItem expected = TestHelper.GetObjectFromJsonFile<OrderItem>("order_item_full.json");
            Assert.Equal(expected, orderItemFull);
        }

        [Fact]
        public void AsJsonNegative()
        {
            OrderItem expected = TestHelper.GetObjectFromJsonFile<OrderItem>("order_item_negative.json");
            Assert.Equal(expected, orderItemNegative);
        }

        private void AssertRequiredFields(OrderItem orderItem)
        {
            Assert.Equal(ItemCategory.PHYSICAL, orderItem.Category);
            Assert.Equal(Currency.EUR, orderItem.Amount.Currency);
            Assert.Equal(1000L, orderItem.Amount.GetAmountInCents());
            Assert.Equal("Description", orderItem.Description);
            Assert.Equal("Test product", orderItem.Name);
            Assert.Equal(1, orderItem.Quantity);
        }

        private void AssertOptionalFields(OrderItem orderItem)
        {
            Assert.Equal(Currency.EUR, orderItem.Tax.Currency);
            Assert.Equal(100L, orderItem.Tax.GetAmountInCents());
            Assert.Equal("1", orderItem.Id);
            Assert.Equal(VatCategory.LOW, orderItem.VatCategory);
        }
    }
}
