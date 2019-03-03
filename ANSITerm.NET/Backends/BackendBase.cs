using System;
using System.Text;

namespace ANSITerm.Backends
{
    public abstract class BackendBase : IConsoleBackend
    {
        public bool IsInputRedirected
        {
            get => Console.IsInputRedirected;
        }
        public int BufferHeight
        {
            get => Console.BufferHeight;
            set => Console.BufferHeight = value;
        }
        public int BufferWidth
        {
            get => Console.BufferWidth;
            set => Console.BufferWidth = value;
        }
        public bool CapsLock
        {
            get => Console.CapsLock;
        }
        public int CursorLeft
        {
            get => Console.CursorLeft;
            set => Console.CursorLeft = value;
        }
        public int CursorSize
        {
            get => Console.CursorSize;
            set => Console.CursorSize = value;
        }
        public int CursorTop
        {
            get => Console.CursorTop;
            set => Console.CursorTop = value;
        }
        public bool CursorVisible
        {
            get => Console.CursorVisible;
            set => Console.CursorVisible = value;
        }
        public Encoding InputEncoding
        {
            get => Console.InputEncoding;
            set => Console.InputEncoding = value;
        }
        public bool IsErrorRedirected => Console.IsErrorRedirected;
        public int WindowWidth
        {
            get => Console.WindowWidth;
            set => Console.WindowWidth = value;
        }
        public bool IsOutputRedirected => Console.IsOutputRedirected;
        public bool KeyAvailable => Console.KeyAvailable;
        public int LargestWindowHeight => Console.LargestWindowHeight;
        public int LargestWindowWidth => Console.LargestWindowWidth;
        public bool NumberLock => Console.NumberLock;
        public Encoding OutputEncoding
        {
            get => Console.OutputEncoding;
            set => Console.OutputEncoding = value;
        }
        public string Title
        {
            get => Console.Title;
            set => Console.Title = value;
        }
        public int WindowHeight
        {
            get => Console.WindowHeight;
            set => Console.WindowHeight = value;
        }
        public int WindowLeft
        {
            get => Console.WindowLeft;
            set => Console.WindowLeft = value;
        }
        public int WindowTop
        {
            get => Console.WindowTop;
            set => Console.WindowTop = value;
        }
        public abstract ColorValue ForegroundColor { set; }
        public abstract ColorValue BackgroundColor { set; }
        public ColorMode ColorMode { get; set; } = ColorMode.Color8;

        public void Clear() => Console.Clear();

        public abstract bool IsColorModeAvailable(ColorMode mode);

        public int Peek() => Console.In.Peek();
        public int Read() => Console.Read();
        public ConsoleKeyInfo ReadKey() => Console.ReadKey();

        public void SetCursorPosition(int x, int y) => Console.SetCursorPosition(x, y);

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

        public void ResetColor() => Console.ResetColor();
    }
}