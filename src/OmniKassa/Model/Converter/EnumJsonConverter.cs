using System;
using Newtonsoft.Json;

namespace OmniKassa.Model
{
    /// <summary>
    /// Specialized <see cref="JsonConverter"/> class which converts an enum to string and vice versa
    /// </summary>
    /// <typeparam name="T">Enum type</typeparam>
    public class EnumJsonConverter<T> : JsonConverter where T : struct, IConvertible
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
        /// Reads the JSON representation of a Enum
        /// </summary>
        /// <param name="reader">The Newtonsoft.Json.JsonReader to read from.</param>
        /// <param name="objectType">Type of the object.</param>
        /// <param name="existingValue">The existing value of object being read.</param>
        /// <param name="serializer">The calling serializer.</param>
        /// <returns>The object value.</returns>
        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            string value = (string)reader.Value;
            if (Enum.TryParse(value, true, out T result))
            {
                return result;
            }
            return null;
        }

        /// <summary>
        /// Writes the JSON representation of the object.
        /// </summary>
        /// <param name="writer">The Newtonsoft.Json.JsonWriter to write to.</param>
        /// <param name="value">The value.</param>
        /// <param name="serializer">The calling serializer.</param>
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            if (value != null)
            {
                T enumValue = (T)value;
                writer.WriteValue(enumValue.ToString());
            }
            else
            {
                writer.WriteNull();
            }
        }
    }
}
