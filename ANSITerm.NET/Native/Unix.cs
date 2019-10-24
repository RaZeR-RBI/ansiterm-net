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
		// stdin key check handling
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

		// terminal window resize handling
		private const int SIGWINCH = 28;

		[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
		delegate void SignalCallback(int unused);

		[DllImport("libc")]
		private static extern IntPtr signal(
			int signum,
			[MarshalAs(UnmanagedType.FunctionPtr)] SignalCallback cb);

		public static EventHandler OnTerminalResize { get; set; }

		static Unix()
		{
			TryInstallStdinHandler();
			TryInstallResizeHandler();
		}

		static void TryInstallStdinHandler()
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

		static SignalCallback s_cb = (_) =>
		{
			OnTerminalResize?.Invoke(null, EventArgs.Empty);
		};

		static void TryInstallResizeHandler()
		{
			try
			{
				signal(SIGWINCH, s_cb);
			}
			catch (Exception ex)
			{
				Console.Error.WriteLine(
					"Cannot install SIGWINCH hook, terminal resizing event" +
					$" will not work. Reason: {ex}");
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