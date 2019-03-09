using System;
using System.Drawing;

namespace ANSITerm.Backends
{
    public class StdBackend : BackendBase
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

        public override int CursorLeft => Console.CursorLeft;

        public override int CursorTop => Console.CursorTop;

        public override Point CursorPosition => new Point(Console.CursorLeft,Console.CursorTop);
    }
}