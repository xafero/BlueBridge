using BlueBridgeNet.API;
using log4net;
using System;
using System.Runtime.InteropServices;
using System.Threading;

namespace BlueBridgeNet.Linux
{
    public class LinuxBridge : IBridge
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(LinuxBridge).Name);

        public event EventHandler<IBlueEvent> OnBlueEvent;

        private bool shouldRun = true;
        private Thread watcher;

        public void Start()
        {
            log.Debug($"Hub initialising...");
            var devId = Bluez.hci_get_route(IntPtr.Zero);
            log.Debug($"Device ID = {devId}");
            var sockFd = Linux.socket(Consts.AF_BLUETOOTH, Consts.SOCK_RAW | Consts.SOCK_CLOEXEC, Consts.BTPROTO_HCI);
            log.Debug($"Socket FD = {sockFd}");
            var addr = new Sockaddr_hci
            {
                hci_family = Consts.AF_BLUETOOTH,
                hci_dev = (ushort)devId,
                hci_channel = Consts.HCI_CHANNEL_RAW
            };
            var status = Linux.bind(sockFd, ref addr, Marshal.SizeOf(addr));
            log.Debug($"Socket bind => {status}");
            status = Bluez.hci_le_set_scan_parameters(sockFd, 0, 0x10, 0x10, 0, 0, 1000);
            log.Debug($"Bluetooth LE scan parameters => {status}");
            var filter = new Hci_filter
            {
                type_mask = 0x00000010,
                event_mask = 0x4000000000000000ul,
                opcode = 0
            };
            status = Linux.setsockopt(sockFd, Consts.SOL_HCI, Consts.HCI_FILTER, ref filter, Marshal.SizeOf(filter));
            log.Debug($"Socket options => {status}");
            status = Bluez.hci_le_set_scan_enable(sockFd, 1, 0, 1000);
            log.Debug($"Bluetooth LE scan enable => {status}");
            watcher = new Thread(() =>
                        {
                        while (shouldRun)
                        {
                            var bytes = new byte[44];
#if !WINRT
                                Mono.Unix.Native.Syscall.recv(sockFd, bytes, (ulong)bytes.Length, 0);
#endif
                                log.Debug($"Got {bytes.Length} bytes!");
                                OnBlueEvent?.Invoke(this, new BlueEvent
                                {
                                    TimeStamp = DateTime.UtcNow,
                                    Advertisement = new Advertisement
                                    {
                                        // Address = addr,
                                        AddressType = AddressType.Random,
                                        // RSSI = args.RawSignalStrengthInDBm,
                                        // Connectable = IsConnectable(args.AdvertisementType),
                                        // Name = args.Advertisement.LocalName,
                                        // Services = bytes,
                                        // ID = addr,
                                        // UUID = addr,
                                        // ManufacturerData = manu,
                                        ServiceData = bytes
                                    }
                                });
                            }
                        })
            {
                IsBackground = true,
                Name = "Watcher"
            };
            watcher.Start();
            log.Debug("Watcher started.");
        }

        public void Stop()
        {
            log.Debug($"Hub disposing...");
            shouldRun = false;
            watcher.Interrupt();
            log.Debug($"Watcher interrupted.");
        }

        public void Dispose()
        {
            Stop();
        }
    }
}