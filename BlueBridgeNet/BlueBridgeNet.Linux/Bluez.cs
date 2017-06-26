using System;
using System.Runtime.InteropServices;

namespace BlueBridgeNet.Linux
{
    static class Bluez
    {
        [DllImport("libbluetooth.so.3", SetLastError = true)]
        public static extern int hci_get_route(IntPtr addr);

        [DllImport("libbluetooth.so.3", SetLastError = true)]
        public static extern int hci_le_set_scan_parameters(int dev_id, byte type, UInt16 interval, UInt16 window, byte own_type, byte filter, int to);

        [DllImport("libbluetooth.so.3", SetLastError = true)]
        public static extern int hci_le_set_scan_enable(int dev_id, byte enable, byte filter_dup, int to);
    }
}