using System;
using ANSITerm;

namespace FullscreenSample
{
    class Program
    {
        static void Main(string[] args)
        {
            var term = ANSIConsole.GetInstance();
            term.SetFullscreen(true);
            term.SetCursorPosition(10, 10);
            term.Write("Hello! This app should use alternative screen buffer");
            term.SetCursorPosition(10, 20);
            term.Write("Press ENTER to exit");
            term.ReadLine();
            term.SetFullscreen(false);
        }
    }
}
