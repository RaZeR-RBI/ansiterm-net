using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Text;

/// <summary>
/// Defines console backends.
/// </summary>
namespace ANSITerm.Backends
{
	internal abstract class BackendBase : IConsoleBackend
	{
		public bool IsInputRedirected => Console.IsInputRedirected;
		public abstract int CursorLeft { get; }
		public abstract int CursorTop { get; }
		public bool CursorVisible
		{
			set => Console.CursorVisible = value;
		}
		public Encoding InputEncoding
		{
			get => Console.InputEncoding;
			set => Console.InputEncoding = value;
		}
		public bool IsErrorRedirected => Console.IsErrorRedirected;
		public bool IsOutputRedirected => Console.IsOutputRedirected;
		public virtual bool KeyAvailable => Console.KeyAvailable;
		public int LargestWindowHeight => Console.LargestWindowHeight;
		public int LargestWindowWidth => Console.LargestWindowWidth;
		public Encoding OutputEncoding
		{
			get => Console.OutputEncoding;
			set => Console.OutputEncoding = value;
		}
		public string Title
		{
			set => Console.Title = value;
		}
		public int WindowWidth => GetWindowWidth();
		public int WindowHeight => GetWindowHeight();
		public Size WindowSize => new Size(WindowWidth, WindowHeight);
		public abstract ColorValue ForegroundColor { set; }
		public abstract ColorValue BackgroundColor { set; }
		public ColorMode ColorMode { get; set; } = ColorMode.Color8;

		protected ColorMode BestMode { get; private set; }

		public abstract Point CursorPosition { get; }
		public EventHandler Resized { get; set; }

		public BackendBase()
		{
			Detector.Setup();
			ColorMode = BestMode = Detector.GetBestColorMode();
		}

		public void Clear() => Console.Clear();

		public bool IsColorModeAvailable(ColorMode mode) => mode <= BestMode;

		public int Peek() => Console.In.Peek();
		public int Read() => Console.In.Read();
		public virtual ConsoleKeyInfo ReadKey() => Console.ReadKey();
		public virtual ConsoleKeyInfo ReadKey(bool intercept) => Console.ReadKey(intercept);

		public abstract void SetCursorPosition(int x, int y);

		public bool TrySetColorMode(ColorMode mode)
		{
			if (IsColorModeAvailable(mode))
			{
				ColorMode = mode;
				return true;
			}
			return false;
		}

		// note: ToArray() is needed because Console API doesn't work with
		// spans and/or pointers. Sadly, it performs a heap allocation :(
		// https://github.com/dotnet/corefx/blob/v2.0.8/src/System.Memory/src/System/ReadOnlySpan.cs#L314
		public virtual void Write(ReadOnlySpan<char> data) =>
			Console.Out.Write(data.ToArray());

		public virtual void Write(string data) =>
			Console.Out.Write(data);

		public void WriteLine(ReadOnlySpan<char> data)
		{
			Console.Out.Write(data.ToArray());
			Console.Out.WriteLine();
		}

		public void WriteLine(string data) => Console.Out.WriteLine(data);

		public virtual void WriteError(ReadOnlySpan<char> data) =>
			Console.Error.Write(data.ToArray());

		public virtual void WriteError(string data) =>
			Console.Error.Write(data);

		public void WriteErrorLine(ReadOnlySpan<char> data)
		{
			Console.Error.Write(data.ToArray());
			Console.Error.WriteLine();
		}

		public void WriteErrorLine(string data) =>
			Console.Error.WriteLine(data);

		public virtual void ResetColor() => Console.ResetColor();

		public virtual void MoveCursor(Direction direction, int steps)
		{
			var top = CursorTop;
			var left = CursorLeft;
			switch (direction)
			{
				case Direction.Up:
					top -= steps; break;
				case Direction.Down:
					top += steps; break;
				case Direction.Backward:
					left -= steps; break;
				case Direction.Forward:
					left += steps; break;
			}
			SetCursorPosition(left, top);
		}

		public void SetCursorPosition(Point p) => SetCursorPosition(p.X, p.Y);

		protected virtual int GetWindowWidth() => Console.WindowWidth;
		protected virtual int GetWindowHeight() => Console.WindowHeight;

		public string ReadLine() => Console.ReadLine();

		public abstract void SetFullscreen(bool value);
	}
}