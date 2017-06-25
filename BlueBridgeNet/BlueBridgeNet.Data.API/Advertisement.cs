namespace BlueBridgeNet.API
{
    public class Advertisement : IAdvertisement
    {
        public string ID { get; set; }

        public string Address { get; set; }

        public AddressType AddressType { get; set; }

        public string UUID { get; set; }

        public string[] Services { get; set; }

        public bool Connectable { get; set; }

        public int RSSI { get; set; }

        public byte[] ServiceData { get; set; }

        public byte[] ManufacturerData { get; set; }

        public string Name { get; set; }

        public int? PowerLevel { get; set; }
    }
}