using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace BlueBridgeNet.Json
{
    public static class JsonConfig
    {
        public static JsonSerializerSettings GetDefault()
            => new JsonSerializerSettings
            {
                Converters = {
                    new StringEnumConverter(),
                    new HexBytesConverter()
                },
                DefaultValueHandling = DefaultValueHandling.Ignore,
                NullValueHandling = NullValueHandling.Ignore
            };
    }
}