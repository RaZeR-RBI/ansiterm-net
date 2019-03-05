using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace ANSITerm.Backends
{
    public class ANSIBackend : BackendBase
    {

        internal ANSIBackend() : base()
        {
            CheckIfWidthAndHeightIsAvailable();
        }

        public override ColorValue ForegroundColor
        {
            set
            {
                if (value.Mode != ColorMode)
                    value.Transform(ColorMode);
                switch (value.Mode)
                {
                    case ColorMode.Color8:
                    case ColorMode.Color16:
                        Console.ForegroundColor = ColorUtil.AsConsoleColor(value); break;
                    case ColorMode.Color256:
                        Set8BitColor(value, false); break;
                    case ColorMode.TrueColor:
                        SetTrueColor(value, false); break;
                }
            }
        }

        public override ColorValue BackgroundColor
        {
            set
            {
                if (value.Mode != ColorMode)
                    value.Transform(ColorMode);
                switch (value.Mode)
                {
                    case ColorMode.Color8:
                    case ColorMode.Color16:
                        Console.BackgroundColor = ColorUtil.AsConsoleColor(value); break;
                    case ColorMode.Color256:
                        Set8BitColor(value, true); break;
                    case ColorMode.TrueColor:
                        SetTrueColor(value, true); break;
                }
            }
        }


        private void Set8BitColor(ColorValue color, bool isBackground)
        {
            var code = (isBackground ? 48 : 38);
            var index = color.RawValue;
            Write($"\x1B[{code};5;{index}m");
        }

        private void SetTrueColor(ColorValue color, bool isBackground)
        {
            var r = (color.RawValue & 0x00FF0000) >> 16;
            var g = (color.RawValue & 0x0000FF00) >> 8;
            var b = (color.RawValue & 0x000000FF);
            var code = (isBackground ? 48 : 38);
            Write($"\x1B[{code};2;{r};{g};{b}m");
        }

        public override void MoveCursor(Direction direction, int steps)
        {
            char code = 'A';
            switch (direction)
            {
                case Direction.Up: code = 'A'; break;
                case Direction.Down: code = 'B'; break;
                case Direction.Forward: code = 'C'; break;
                case Direction.Backward: code = 'D'; break;
            }
            Write($"\x1B[{steps}{code}");
        }

        public override void ResetColor() => Write("\x1B[0m");



        private bool _windowSizeFromEnv = false;
        private int _windowWidthFromEnv = 0;
        private int _windowHeightFromEnv = 0;

        protected override int GetWindowWidth() => _windowSizeFromEnv ?
            _windowWidthFromEnv : Console.WindowWidth;

        protected override int GetWindowHeight() => _windowSizeFromEnv ?
            _windowHeightFromEnv : Console.WindowHeight;

        private void WindowSizeFallback()
        {
            var sources = new Dictionary<string, Action>() {
                {"environment variables", GetWindowSizeFromEnv },
                {"stty", GetWindowSizeFromStty }
            };
            foreach(var pair in sources)
            {
                WriteErrorLine($"Trying to get window size from {pair.Key}...");
                try
                {
                    pair.Value();
                    return;
                }
                catch (Exception ex)
                {
                    WriteErrorLine($"Failed, reason: {ex}");
                }
            }
            throw new Exception("Could not obtain terminal window size");
        }

        private void CheckIfWidthAndHeightIsAvailable()
        {
            try
            {
                var cols = Console.WindowWidth;
                var rows = Console.WindowHeight;
                if (cols == 0 || rows == 0)
                    throw new ArgumentOutOfRangeException("Invalid size reported from terminfo");
            } catch (Exception ex) {
                _windowSizeFromEnv = true;
                WriteErrorLine($"Could not obtain window size, falling back to $COLUMNS and $ROWS. Reason: {ex}");
                WindowSizeFallback(); 
            }
        }

        private void GetWindowSizeFromStty()
        {
            var startInfo = new ProcessStartInfo("stty", "size") {
                UseShellExecute = false,
                RedirectStandardOutput = true
            };
            var process = new Process() {
                StartInfo = startInfo
            };
            process.Start();
            var output = process.StandardOutput.ReadToEnd().Split(' ');
            _windowWidthFromEnv = int.Parse(output[1]);
            _windowHeightFromEnv = int.Parse(output[1]);
        }

        private void GetWindowSizeFromEnv()
        {
            var w = Environment.GetEnvironmentVariable("COLUMNS");
            var h = Environment.GetEnvironmentVariable("ROWS");
            if (w == null || h == null)
                throw new ArgumentException("Environment variables COLUMNS and ROWS are not set");
            _windowWidthFromEnv = int.Parse(w);
            _windowHeightFromEnv = int.Parse(h);
        }

    }
}