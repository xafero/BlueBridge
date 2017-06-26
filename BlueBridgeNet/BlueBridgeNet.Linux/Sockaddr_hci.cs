using System;
using System.Runtime.InteropServices;

namespace BlueBridgeNet.Linux
{
    [StructLayout(LayoutKind.Sequential)]
    struct Sockaddr_hci
    {
        public UInt16 hci_family;
        public ushort hci_dev;
        public ushort hci_channel;
    }
}