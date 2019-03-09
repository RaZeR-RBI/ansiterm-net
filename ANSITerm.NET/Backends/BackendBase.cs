using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Text;

namespace ANSITerm.Backends
{
    public abstract class BackendBase : IConsoleBackend
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
        public bool KeyAvailable => Console.KeyAvailable;
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
        public abstract ColorValue ForegroundColor { set; }
        public abstract ColorValue BackgroundColor { set; }
        public ColorMode ColorMode { get; set; } = ColorMode.Color8;

        protected ColorMode BestMode { get; private set; }

        public abstract Point CursorPosition { get; }

        public BackendBase()
        {
            Detector.Setup();
            ColorMode = BestMode = Detector.GetBestColorMode();
        }

        public void Clear() => Console.Clear();

        public bool IsColorModeAvailable(ColorMode mode) => mode <= BestMode;

        public int Peek() => Console.In.Peek();
        public int Read() => Console.In.Read();
        public ConsoleKeyInfo ReadKey() => Console.ReadKey();
        public ConsoleKeyInfo ReadKey(bool intercept) => Console.ReadKey(intercept);

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

        public virtual void Write(string data) => Console.Out.Write(data);
        public void WriteLine(string data) => Write(data + Environment.NewLine);
        public virtual void WriteError(string data) => Console.Error.Write(data);
        public void WriteErrorLine(string data) => WriteError(data + Environment.NewLine);

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
    }
}