using System;
using System.Threading;
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
            var key = new ConsoleKeyInfo();
            while (key.Key != ConsoleKey.Enter)
            {
                // this loop is for testing a workaround for this issue
                // https://github.com/dotnet/corefx/issues/30610
                while (!term.KeyAvailable)
                    Thread.Sleep(10);

                key = term.ReadKey(true);
                term.SetCursorPosition(10, 30);
                term.Write($"You pressed {key.Key}          ");
            }
            term.SetFullscreen(false);
        }
    }
}
