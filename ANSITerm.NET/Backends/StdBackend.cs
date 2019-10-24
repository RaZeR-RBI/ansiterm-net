using System;
using System.Drawing;

namespace ANSITerm.Backends
{
	internal class StdBackend : BackendBase
	{
		public override ColorValue ForegroundColor
		{
			set
			{
				Console.ForegroundColor = ColorUtil.AsConsoleColor(value);
			}
		}

		public override ColorValue BackgroundColor
		{
			set
			{
				Console.BackgroundColor = ColorUtil.AsConsoleColor(value);
			}
		}

		public override int CursorLeft => Console.CursorLeft - _startPoint.X;

		public override int CursorTop => Console.CursorTop - _startPoint.Y;

		public override Point CursorPosition => new Point(CursorLeft, CursorTop);

		private Point _startPoint = new Point();
		private Size _startBufSize = new Size();
		private Point _appStartPoint = new Point();
		public StdBackend() : base()
		{
			_appStartPoint = _startPoint = new Point(Console.CursorLeft, Console.CursorTop);
			_startBufSize = new Size(Console.BufferWidth, Console.BufferHeight);
		}

		public override void SetCursorPosition(int x, int y) =>
			Console.SetCursorPosition(x + _startPoint.X, y + _startPoint.Y);

		public override void SetFullscreen(bool value)
		{
			var winSize = new Size(WindowWidth, WindowHeight);
			Console.Clear();
			if (value)
			{
				Console.BufferWidth = winSize.Width;
				Console.BufferHeight = winSize.Height;
				_startPoint = new Point();
			}
			else
			{
				Console.BufferWidth = Math.Max(winSize.Width, _startBufSize.Width);
				Console.BufferHeight = Math.Max(winSize.Height, _startBufSize.Height);
				_startPoint = _appStartPoint;
				_startBufSize = new Size(Console.BufferWidth, Console.BufferHeight);
			}
		}
	}
}