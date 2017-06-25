namespace BlueBridgeNet.API
{
    public interface IAdvertisement
    {
        string ID { get; }

        string Address { get; }

        AddressType AddressType { get; }

        string UUID { get; }

        string[] Services { get; }

        bool Connectable { get; }

        int RSSI { get; }

        byte[] ServiceData { get; }

        byte[] ManufacturerData { get; }

        string Name { get; }

        int? PowerLevel { get; }
    }
}