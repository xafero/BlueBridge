using BlueBridgeNet.API;
using System;

namespace BlueBridgeNet.Linux
{
    public class LinuxBridge : IBridge
    {
        public event EventHandler<IBlueEvent> OnBlueEvent;

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public void Start()
        {
            throw new NotImplementedException();
        }

        public void Stop()
        {
            throw new NotImplementedException();
        }
    }
}