using System;
using System.Collections.Generic;
using OmniKassa.Model;
using OmniKassa.Model.Enums;
using OmniKassa.Model.Order;

namespace OmniKassa.Tests.Model.Order
{
    public class OrderItemFactory
    {
        public static OrderItem OrderItemFull()
        {
            return DefaultOrderItemBuilder()
                    .WithId("1")
                    .WithVatCategory(VatCategory.LOW)
                    .WithTax(Money.FromDecimal(Currency.EUR, 1m))
                    .Build();
        }

        public static OrderItem OrderItemNegative()
        {
            return new OrderItem.Builder()
                    .WithQuantity(1)
                    .WithName("Test product")
                    .WithDescription("Description")
                    .WithAmount(Money.FromDecimal(Currency.EUR, -10m))
                    .WithTax(Money.FromDecimal(Currency.EUR, -1m))
                    .WithItemCategory(ItemCategory.PHYSICAL)
                    .Build();
        }

        public static OrderItem OrderItem()
        {
            return DefaultOrderItemBuilder().Build();
        }

        public static OrderItem.Builder DefaultOrderItemBuilder()
        {
            return new OrderItem.Builder()
                    .WithQuantity(1)
                    .WithName("Test product")
                    .WithDescription("Description")
                    .WithAmount(Money.FromDecimal(Currency.EUR, 10.00m))
                    .WithItemCategory(ItemCategory.PHYSICAL);
        }

        public static List<OrderItem> ToList(OrderItem orderItem)
        {
            return new List<OrderItem>() { orderItem };
        }
    }
}
