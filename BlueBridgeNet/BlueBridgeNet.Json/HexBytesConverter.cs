using System;
using Newtonsoft.Json;
using System.Runtime.Remoting.Metadata.W3cXsd2001;

namespace BlueBridgeNet.Json
{
    /// <summary>
    /// Converts a binary value to and from a hex string value.
    /// </summary>
    public class HexBytesConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(byte[]);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            var hex = reader.Value.ToString();
            var bytes = SoapHexBinary.Parse(hex).Value;
            return bytes;
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            var bytes = (byte[])value;
            var hex = new SoapHexBinary(bytes);
            writer.WriteValue(hex.ToString());
        }
    }
}