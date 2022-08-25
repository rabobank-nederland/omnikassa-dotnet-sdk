using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace OmniKassa.Model.Enums
{
    /// <summary>
    /// This enum defines the vat categories of order items.
    /// </summary>
    public enum VatCategory
    {
        /// <summary>
        /// VAT category high
        /// </summary>
        HIGH,

        /// <summary>
        /// VAT category low
        /// </summary>
        LOW,

        /// <summary>
        /// VAT category zero
        /// </summary>
        ZERO,

        /// <summary>
        /// No VAT
        /// </summary>
        NONE,
    }

    /// <summary>
    /// Specialized <see cref="JsonConverter"/> where 
    /// </summary>
    public class VatCategoryJsonConverter : JsonConverter
    {
        /// <summary>
        /// Determines whether this instance can convert the specified object type.
        /// </summary>
        /// <param name="objectType">Type of the object.</param>
        /// <returns>true if this instance can convert the specified object type; otherwise, false.</returns>
        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(string);
        }

        /// <summary>
        /// Reads the JSON representation of a VatCategory
        /// </summary>
        /// <param name="reader">The Newtonsoft.Json.JsonReader to read from.</param>
        /// <param name="objectType">Type of the object.</param>
        /// <param name="existingValue">The existing value of object being read.</param>
        /// <param name="serializer">The calling serializer.</param>
        /// <returns>The object value.</returns>
        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            string value = (string)reader.Value;
            return VatCategories.GetValue(value);
        }

        /// <summary>
        /// Writes the JSON representation of the object.
        /// </summary>
        /// <param name="writer">The Newtonsoft.Json.JsonWriter to write to.</param>
        /// <param name="value">The value.</param>
        /// <param name="serializer">The calling serializer.</param>
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            if (value != null) {
                VatCategory enumValue = (VatCategory)value;
                String strValue = VatCategories.GetValue(enumValue);
                writer.WriteValue(strValue);
            } else {
                writer.WriteNull();
            }
        }
    }

    /// <summary>
    /// Helper class to convert a <see cref="VatCategory"/> to and from its JSON value.
    /// </summary>
    public static class VatCategories {

        private static readonly Dictionary<VatCategory, String> values = new Dictionary<VatCategory, string>() {
            { VatCategory.HIGH, "1"},
            { VatCategory.LOW,  "2"},
            { VatCategory.ZERO, "3"},
            { VatCategory.NONE, "4"}
        };

        /// <summary>
        /// Gets the string representation of a VatCategory
        /// </summary>
        /// <param name="vatCategory">VatCategory</param>
        /// <returns>String representation</returns>
        public static String GetValue(VatCategory? vatCategory) {
            
            if(vatCategory != null) {
                return values[(VatCategory)vatCategory];
            }
            return null;
        }

        /// <summary>
        /// Gets the VatCategory by the string representation
        /// </summary>
        /// <param name="value">String representation</param>
        /// <returns>VatCategory</returns>
        public static VatCategory? GetValue(String value) {
            foreach(KeyValuePair<VatCategory, String> pair in values) {
                if(pair.Value.Equals(value)) {
                    return pair.Key;
                }
            }
            return null;
        }
    }
}
