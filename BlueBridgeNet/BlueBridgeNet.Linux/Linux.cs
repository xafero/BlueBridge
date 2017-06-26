using System;
using System.Runtime.InteropServices;

namespace BlueBridgeNet.Linux
{
    static class Linux
    {
        [DllImport("libc", SetLastError = true)]
        public static extern int socket(int domain, int type, int protocol);

        [DllImport("libc", SetLastError = true)]
        public static extern int bind(int sockfd, ref Sockaddr_hci addr, int addrlen);

        [DllImport("libc", SetLastError = true)]
        public static extern int setsockopt(int socket, int level, int optname, [In, Out] ref Hci_filter optval, int optlen);

        [DllImport("libc", SetLastError = true)]
        public static extern long recvfrom(int sockfd, [In, Out] ref byte[] buf, int len, int flags, IntPtr src_addr, int addrlen);
    }
}