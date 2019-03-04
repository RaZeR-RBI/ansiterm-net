using System;
using System.Drawing;
using System.Text;

namespace ANSITerm
{
    public enum Direction
    {
        Up,
        Down,
        Forward,
        Backward
    }

    public interface IConsoleBackend
    {
        void Clear();
        void SetCursorPosition(int x, int y);
        void SetCursorPosition(Point p);
        void MoveCursor(Direction direction, int steps);
        void Write(string data);
        void WriteLine(string data);
        void WriteError(string data);
        void WriteErrorLine(string data);
        int Peek();
        int Read();
        ConsoleKeyInfo ReadKey();
        ConsoleKeyInfo ReadKey(bool intercept);
        bool IsInputRedirected { get; }
        int BufferHeight { get; }
        int BufferWidth { get; }
        int CursorLeft { get; }
        int CursorTop { get; }
        Point CursorPosition { get; }
        bool CursorVisible { set; }
        Encoding InputEncoding { get; set; }
        bool IsErrorRedirected { get; }
        int WindowWidth { get; }
        bool IsOutputRedirected { get; }
        bool KeyAvailable { get; }
        int LargestWindowHeight { get; }
        int LargestWindowWidth { get; }
        Encoding OutputEncoding { get; set; }
        string Title { set; }
        int WindowHeight { get; }
        int WindowLeft { get; }
        int WindowTop { get; }
        ColorValue ForegroundColor { set; }
        ColorValue BackgroundColor { set; }
        void ResetColor();
        ColorMode ColorMode { get; }
        bool IsColorModeAvailable(ColorMode mode);
        bool TrySetColorMode(ColorMode mode);
    }
}