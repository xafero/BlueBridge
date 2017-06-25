using System;

namespace BlueBridgeNet.API
{
    public class BlueEvent : IBlueEvent
    {
        public DateTimeOffset TimeStamp { get; set; }

        public IAdvertisement Advertisement { get; set; }

        public string Flags { get; set; }
    }
}