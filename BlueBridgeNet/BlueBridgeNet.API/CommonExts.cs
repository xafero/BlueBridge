using System;
using System.Runtime.Remoting.Metadata.W3cXsd2001;

namespace BlueBridgeNet.API
{
    public static class CommonExts
    {
        public static string ToHex(this ulong value) => ToHex(BitConverter.GetBytes(value));

        public static string ToHex(this byte[] bytes) => (new SoapHexBinary(bytes)).ToString();

        public static byte[] FromHex(this string text) => SoapHexBinary.Parse(text).Value;
    }
}