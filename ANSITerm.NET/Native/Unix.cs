using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

/// <summary>
/// Internal namespace for calling native platform functions
/// </summary>
namespace ANSITerm.NET.Native
{
    internal static class Unix
    {
        private struct pollfd
        {
            public int fd;
            public short events;
            public short revents;
        };

        private const int POLLIN = 0x0001;
        private const int STDIN_FILENO = 0;

        [DllImport("libc")]
        private static extern int poll(ref pollfd fds, uint nfds, int timeout);

        private static bool s_pollAvailable = false;

        static Unix()
        {
            try
            {
                PollStdin();
                s_pollAvailable = true;
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine(
                    "Cannot use libc poll to query stdin, falling back to corefx" +
                    $" implementation. Reason: {ex}");
            }
        }

        public static bool PollStdin()
        {
            if (!s_pollAvailable) return false;
            var fd = new pollfd()
            {
                fd = STDIN_FILENO,
                events = POLLIN
            };
            var result = poll(ref fd, 1, 0);
            return result > 0;
        }
    }
}