using System;
using Newtonsoft.Json;
using OmniKassa.Model.Enums;

namespace OmniKassa.Model.Order
{
    /// <summary>
    /// Item (with quantity) in an order
    /// </summary>
    [JsonObject(MemberSerialization.OptIn)]
    public class OrderItem
    {
        /// <summary>
        /// Item quantity
        /// </summary>
        [JsonProperty(PropertyName = "quantity")]
        public int Quantity { get; private set; }

        /// <summary>
        /// Item ID
        /// </summary>
        [JsonProperty(PropertyName = "id")]
        public String Id { get; private set; }

        /// <summary>
        /// Item name
        /// </summary>
        [JsonProperty(PropertyName = "name")]
        public String Name { get; private set; }

        /// <summary>
        /// Item description
        /// </summary>
        [JsonProperty(PropertyName = "description")]
        public String Description { get; private set; }
       
        /// <summary>
        /// Amount per single item, includes VAT
        /// </summary>
        [JsonProperty(PropertyName = "amount")]
        public Money Amount { get; private set; }

        /// <summary>
        /// VAT per single item
        /// </summary>
        [JsonProperty(PropertyName = "tax")]
        public Money Tax { get; private set; }

        /// <summary>
        /// Item category
        /// </summary>
        [JsonProperty(PropertyName = "category")]
        [JsonConverter(typeof(EnumJsonConverter<ItemCategory>))]
        public ItemCategory? Category { get; private set; }

        /// <summary>
        /// Item VAT category
        /// </summary>
        [JsonProperty(PropertyName = "vatCategory")]
        [JsonConverter(typeof(VatCategoryJsonConverter))]
        public VatCategory? VatCategory { get; private set; }

        /// <summary>
        /// Initializes an empty OrderItem
        /// </summary>
        public OrderItem()
        {
            
        }

        /// <summary>
        /// Initializes an OrderItem using the Builder
        /// </summary>
        /// <param name="builder">Builder</param>
        public OrderItem(Builder builder)
        {
            this.Id = builder.Id;
            this.Quantity = builder.Quantity;
            this.Name = builder.Name;
            this.Description = builder.Description;
            this.Amount = builder.Amount;
            this.Tax = builder.Tax;
            this.Category = builder.Category;
            this.VatCategory = builder.VatCategory;
        }

        /// <summary>
        /// Determines whether the specified object is equal to the current object.
        /// </summary>
        /// <param name="obj">The object to compare with the current object.</param>
        /// <returns>true if the specified object is equal to the current object; otherwise, false.</returns>
        public override Boolean Equals(object obj)
        {
            if (this == obj)
            {
                return true;
            }
            if (obj == null)
            {
                return false;
            }
            if (!(obj is OrderItem))
            {
                return false;
            }
            OrderItem order = (OrderItem)obj;
            return Equals(Quantity, order.Quantity) &&
                Equals(Id, order.Id) &&
                Equals(Name, order.Name) &&
                Equals(Description, order.Description) &&
                Equals(Amount, order.Amount) &&
                Equals(Tax, order.Tax) &&
                Equals(Category, order.Category) &&
                Equals(VatCategory, order.VatCategory);
        }

        /// <summary>
        /// Serves as the default hash function.
        /// </summary>
        /// <returns>A hash code for the current object.</returns>
        public override int GetHashCode()
        {
            unchecked // Overflow is fine, just wrap
            {
                int hash = 0x51ed270b;
                hash = (hash * -1521134295) + Quantity.GetHashCode();
                hash = (hash * -1521134295) + (Id == null ? 0 : Id.GetHashCode());
                hash = (hash * -1521134295) + (Name == null ? 0 : Name.GetHashCode());
                hash = (hash * -1521134295) + (Description == null ? 0 : Description.GetHashCode());
                hash = (hash * -1521134295) + (Amount == null ? 0 : Amount.GetHashCode());
                hash = (hash * -1521134295) + (Tax == null ? 0 : Tax.GetHashCode());
                hash = (hash * -1521134295) + (Category == null ? 0 : Category.GetHashCode());
                hash = (hash * -1521134295) + (VatCategory == null ? 0 : VatCategory.GetHashCode());
                return hash;
            }
        }

        /// <summary>
        /// OrderItem builder
        /// </summary>
        public class Builder
        {
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
            public int Quantity { get; private set; }
            public String Id { get; private set; }
            public String Name { get; private set; }
            public Money Amount { get; private set; }
            public Money Tax { get; private set; }
            public ItemCategory? Category { get; private set; }
            public VatCategory? VatCategory { get; private set; }
            public String Description { get; private set; }
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member

            /// <summary>
            /// - Optional
            /// - Maximum length of 25 characters
            /// </summary>
            /// <param name="id">Item ID</param>
            /// <returns>Builder</returns>
            public Builder WithId(String id)
            {
                this.Id = id;
                return this;
            }

            /// <summary>
            /// - Must be greater than zero
            /// </summary>
            /// <param name="quantity">Item quantity</param>
            /// <returns>Builder</returns>
            public Builder WithQuantity(int quantity)
            {
                this.Quantity = quantity;
                return this;
            }

            /// <summary>
            /// - Must not be null
            /// - Must only contain alphanumeric characters
            /// - Maximum length of 50 characters
            /// </summary>
            /// <param name="name">Item name</param>
            /// <returns>Builder</returns>
            public Builder WithName(String name)
            {
                this.Name = name;
                return this;
            }

            /// <summary>
            /// - Should not be null or empty
            /// - Maximum length of 100 characters
            /// </summary>
            /// <param name="description">Item description</param>
            /// <returns>Builder</returns>
            public Builder WithDescription(String description)
            {
                this.Description = description;
                return this;
            }

            /// <summary>
            /// - Must not be null
            /// - Must have the same Currency as tax
            /// </summary>
            /// <param name="amount">Amount per single item, includes VAT</param>
            /// <returns>Builder</returns>
            public Builder WithAmount(Money amount)
            {
                this.Amount = amount;
                return this;
            }

            /// <summary>
            /// - Must have the same Currency as amount
            /// </summary>
            /// <param name="tax">VAT per single item</param>
            /// <returns>Builder</returns>
            public Builder WithTax(Money tax)
            {
                this.Tax = tax;
                return this;
            }

            /// <summary>
            /// - Must not be null
            /// - Must be a valid category
            /// </summary>
            /// <param name="category">Item category</param>
            /// <returns>Builder</returns>
            public Builder WithItemCategory(ItemCategory? category)
            {
                this.Category = category;
                return this;
            }

            /// <summary>
            /// - Optional
            /// - Must be a valid VatCategory
            /// </summary>
            /// <param name="vatCategory">VAT category</param>
            /// <returns>Builder</returns>
            public Builder WithVatCategory(VatCategory? vatCategory)
            {
                this.VatCategory = vatCategory;
                return this;
            }

            /// <summary>
            /// Initializes and returns an OrderItem
            /// </summary>
            /// <returns>OrderItem</returns>
            public OrderItem Build()
            {
                return new OrderItem(this);
            }
        }
    }
}
