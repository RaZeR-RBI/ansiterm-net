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

        public override int CursorLeft => Console.CursorLeft - _startPoint.X;

        public override int CursorTop => Console.CursorTop - _startPoint.Y;

        public override Point CursorPosition => new Point(CursorLeft, CursorTop);

        private Point _startPoint = new Point();
        public StdBackend(): base()
        {
            _startPoint = new Point(Console.CursorLeft, Console.CursorTop);
        }

        public override void SetCursorPosition(int x, int y) =>
            Console.SetCursorPosition(x + _startPoint.X, y + _startPoint.Y);
    }
}