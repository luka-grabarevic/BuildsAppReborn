using System;
using System.Linq;
using Newtonsoft.Json;

namespace BuildsAppReborn.Infrastructure
{
    // https://stackoverflow.com/a/22755077
    public class TolerantEnumConverter : JsonConverter
    {
        public override Boolean CanConvert(Type objectType)
        {
            var type = IsNullableType(objectType) ? Nullable.GetUnderlyingType(objectType) : objectType;
            return type != null && type.IsEnum;
        }

        public override Object ReadJson(JsonReader reader, Type objectType, Object existingValue, JsonSerializer serializer)
        {
            var isNullable = IsNullableType(objectType);
            var enumType = isNullable ? Nullable.GetUnderlyingType(objectType) : objectType;

            var names = Enum.GetNames(enumType);

            if (reader.TokenType == JsonToken.String)
            {
                var enumText = reader.Value.ToString();

                if (!String.IsNullOrEmpty(enumText))
                {
                    var match = names.FirstOrDefault(n => String.Equals(n, enumText, StringComparison.OrdinalIgnoreCase));

                    if (match != null)
                    {
                        return Enum.Parse(enumType, match);
                    }
                }
            }
            else if (reader.TokenType == JsonToken.Integer)
            {
                var enumVal = Convert.ToInt32(reader.Value);
                var values = (Int32[]) Enum.GetValues(enumType);
                if (values.Contains(enumVal))
                {
                    return Enum.Parse(enumType, enumVal.ToString());
                }
            }

            if (!isNullable)
            {
                var defaultName = names.FirstOrDefault(n => String.Equals(n, "Unknown", StringComparison.OrdinalIgnoreCase));

                if (defaultName == null)
                {
                    defaultName = names.First();
                }

                return Enum.Parse(enumType, defaultName);
            }

            return null;
        }

        public override void WriteJson(JsonWriter writer, Object value, JsonSerializer serializer)
        {
            writer.WriteValue(value.ToString());
        }

        private Boolean IsNullableType(Type t)
        {
            return t.IsGenericType && t.GetGenericTypeDefinition() == typeof(Nullable<>);
        }
    }
}