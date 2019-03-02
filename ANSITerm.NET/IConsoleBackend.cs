using System;
using System.Text;

namespace ANSITerm
{
    public interface IConsoleBackend
    {
        void Clear();
        void SetCursorPosition(int x, int y);
        void Write(string data);
        void WriteError(string data);
        int Peek();
        int Read();
        ConsoleKeyInfo ReadKey();
        bool IsInputRedirected { get; }
        int BufferHeight { get; set; }
        int BufferWidth { get; set; }
        bool CapsLock { get; }
        int CursorLeft { get; set; }
        int CursorSize { get; set; }
        int CursorTop { get; set; }
        bool CursorVisible { get; set; }
        Encoding InputEncoding { get; set; }
        bool IsErrorRedirected { get; }
        int WindowWidth { get; set; }
        bool IsOutputRedirected { get; }
        bool KeyAvailable { get; }
        int LargestWindowHeight { get; }
        int LargestWindowWidth { get; }
        bool NumberLock { get; }
        Encoding OutputEncoding { get; set; }
        string Title { get; set; }
        int WindowHeight { get; set; }
        int WindowLeft { get; set; }
        int WindowTop { get; set; }
        ColorValue ForegroundColor { get; set; }
        ColorValue BackgroundColor { get; set; }
    }
}