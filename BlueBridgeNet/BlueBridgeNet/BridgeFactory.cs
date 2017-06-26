using BlueBridgeNet.API;
using System;

namespace BlueBridgeNet
{
    public class BridgeFactory
    {
        public IBridge Build()
        {
            string typeName;
            var isApple = Environment.GetEnvironmentVariable("Apple_PubSub_Socket_Render") != null;
            if (isApple)
                typeName = "BlueBridgeNet.Mac.MacBridge, BlueBridgeNet.Mac";
            else if (Environment.OSVersion.Platform == PlatformID.Unix)
                typeName = "BlueBridgeNet.Linux.LinuxBridge, BlueBridgeNet.Linux";
            else
                typeName = "BlueBridgeNet.WinRT.WindowsBridge, BlueBridgeNet.WinRT";
            var hubType = Type.GetType(typeName);
            return hubType == null ? null : (IBridge)Activator.CreateInstance(hubType);
        }
    }
}