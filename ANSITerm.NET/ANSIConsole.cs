using System;
using ANSITerm.Backends;

/// <summary>
/// This library provides cross-platform terminal access capabilities.
/// </summary>
/// <example>
/// <code>
/// using ANSITerm;
///
/// public static void Main(string[] args)
/// {
///     var term = ANSIConsole.GetInstance();
///     term.ForegroundColor = new ColorValue(Color8.Red);
///     term.WriteLine("Hello, world!");
/// }
///
/// </code>
/// </example>
namespace ANSITerm
{
	/// <summary>
	/// The main class which should be used to get or create a
	/// terminal instance.
	/// </summary>
	public static class ANSIConsole
	{
		private static IConsoleBackend s_instance = null;
		/// <summary>
		/// Gets or creates a terminal instance. The capabilities
		/// are detected based on platform and environment variables.
		/// </summary>
		/// <seealso cref="IConsoleBackend" />
		public static IConsoleBackend GetInstance()
		{
			if (s_instance == null)
				s_instance = DetectAndCreate();
			return s_instance;
		}

		private static IConsoleBackend DetectAndCreate()
		{
			if (Detector.IsStdOnly())
				return new StdBackend();
			return new ANSIBackend();
		}
	}
}
