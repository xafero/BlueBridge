using System;
using BlueBridgeNet.API;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
#if WINRT
using Windows.Devices.Bluetooth.Advertisement;
#endif

namespace BlueBridgeNet.WinRT
{
    public class WindowsBridge : IBridge
    {
#if WINRT
        private readonly BluetoothLEAdvertisementWatcher watcher;
#endif

        public event EventHandler<IBlueEvent> OnBlueEvent;

        public WindowsBridge()
        {
#if WINRT
            watcher = new BluetoothLEAdvertisementWatcher()
            {
                ScanningMode = BluetoothLEScanningMode.Passive
            };
            watcher.Received += Watcher_Received;
#endif
        }

        public void Start()
        {
#if WINRT
            watcher.Start();
#endif
        }

        public void Stop()
        {
#if WINRT
            watcher.Stop();
#endif
        }

        public void Dispose()
        {
            Stop();
        }

#if WINRT
        private void Watcher_Received(BluetoothLEAdvertisementWatcher sender, BluetoothLEAdvertisementReceivedEventArgs args)
        {
            var addr = CommonExts.ToHex(args.BluetoothAddress);
            var uuids = args.Advertisement.ServiceUuids;
            var manu = args.Advertisement.ManufacturerData.Select(
                d => d.Data.ToArray()).SelectMany(b => b).ToArray();
            var data = args.Advertisement.DataSections.Select(
                d => d.Data.ToArray()).SelectMany(b => b).ToArray();
            OnBlueEvent?.Invoke(this, new BlueEvent
            {
                TimeStamp = args.Timestamp,
                Flags = args.Advertisement.Flags.ToString(),
                Advertisement = new Advertisement
                {
                    Address = addr,
                    AddressType = AddressType.Random,
                    RSSI = args.RawSignalStrengthInDBm,
                    Connectable = IsConnectable(args.AdvertisementType),
                    Name = args.Advertisement.LocalName,
                    Services = uuids.Select(u => u.ToString()).ToArray(),
                    ID = addr,
                    UUID = addr,
                    ManufacturerData = manu,
                    ServiceData = data
                }
            });
        }

        private bool IsConnectable(BluetoothLEAdvertisementType type)
                => type == BluetoothLEAdvertisementType.ConnectableDirected
                || type == BluetoothLEAdvertisementType.ConnectableUndirected;
#endif
    }
}