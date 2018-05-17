using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace BuildsAppReborn.Infrastructure
{
    public class InterfaceTypeConverter<TClass, TInterface> : JsonConverter
        where TClass : class, TInterface
    {
        public override Boolean CanConvert(Type objectType)
        {
            return objectType == typeof(TInterface);
        }

        public override Object ReadJson(JsonReader reader, Type objectType, Object existingValue, JsonSerializer serializer)
        {
            var token = JToken.Load(reader);
            return token.Type == JTokenType.Object ? token.ToObject<TClass>() : null;
        }

        public override void WriteJson(JsonWriter writer, Object value, JsonSerializer serializer)
        {
            serializer.Serialize(writer, value);
        }
    }
}