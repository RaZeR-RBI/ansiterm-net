using System;

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
    }
}