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
        int CursorLeft { get; }
        int CursorTop { get; }
        Point CursorPosition { get; }
        bool CursorVisible { set; }
        Encoding InputEncoding { get; set; }
        bool IsErrorRedirected { get; }
        bool IsOutputRedirected { get; }
        bool KeyAvailable { get; }
        Encoding OutputEncoding { get; set; }
        string Title { set; }
        int WindowWidth { get; }
        int WindowHeight { get; }
        ColorValue ForegroundColor { set; }
        ColorValue BackgroundColor { set; }
        void ResetColor();
        ColorMode ColorMode { get; set; }
        bool IsColorModeAvailable(ColorMode mode);
        bool TrySetColorMode(ColorMode mode);
    }
}