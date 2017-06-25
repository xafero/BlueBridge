using System;

namespace BlueBridgeNet.API
{
    public interface IBridge : IDisposable
    {
        void Start();

        event EventHandler<IBlueEvent> OnBlueEvent;

        void Stop();
    }
}