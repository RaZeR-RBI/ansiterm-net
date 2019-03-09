using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Runtime.CompilerServices;

namespace ANSITerm.Backends
{
    public class ANSIBackend : BackendBase
    {

        internal ANSIBackend() : base()
        {
            CheckIfWidthAndHeightIsAvailable();
            CheckIfANSIShouldBeUsedForCursor();
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
                        Set3BitColor(value, false); break;
                    case ColorMode.Color16:
                        Set4BitColor(value, false); break;
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
                        Set3BitColor(value, true); break;
                    case ColorMode.Color16:
                        Set4BitColor(value, true); break;
                    case ColorMode.Color256:
                        Set8BitColor(value, true); break;
                    case ColorMode.TrueColor:
                        SetTrueColor(value, true); break;
                }
            }
        }

        public override int CursorLeft {
            get {
                if (_useANSIForCursor)
                    return CursorPosition.X;
                return Console.CursorLeft;
            }
        }

        public override int CursorTop {
            get {
                if (_useANSIForCursor)
                    return CursorPosition.Y;
                return Console.CursorTop;
            }
        }

        public override Point CursorPosition => _useANSIForCursor ? GetCursorPositionCPR() :
            new Point(Console.CursorLeft, Console.CursorTop);
        
        public override void SetCursorPosition(int x, int y)
        {
            if (_useANSIForCursor)
                Write($"\x1B[{y};{x}H");
            else
                Console.SetCursorPosition(x, y);
        }

        private void Set3BitColor(ColorValue color, bool isBackground)
        {
            var code = (isBackground ? 40 : 30) + color.RawValue;
            Write($"\x1B[{code}m");
        }

        private void Set4BitColor(ColorValue color, bool isBackground)
        {
            var colorIndex = color.RawValue > 7 ? color.RawValue + 52 : color.RawValue;
            var code = (isBackground ? 40 : 30) + colorIndex;
            Write($"\x1B[{code}m");
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


        /* Terminal window size */
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
                RedirectStandardOutput = true,
                UseShellExecute = false 
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


        /* Cursor position query */
        private bool _useANSIForCursor = false;
        private void CheckIfANSIShouldBeUsedForCursor()
        {
            try
            {
                var left = Console.CursorLeft;
                var top = Console.CursorTop;
            } catch (Exception ex) {
                WriteErrorLine($"Falling back to ANSI-based cursor control, reason: {ex}");
                _useANSIForCursor = true;
            }
        }

        private Point GetCursorPositionCPR()
        {
            // let's hope the terminal will respond correctly
            // corefx contains logic for this, but it's Unix-only
            // implementation here is aimed at mintty
            var cur = -1;
            var response = new int[16];
            var offset = (int)'0';
            Write("\x1B[6n");
            while (cur != 0x1B) // skip until CPR response
                cur = ReadNextSymbol();
            while (cur != (int)'[') // wait for [
                cur = ReadNextSymbol();
            
            
            var i = 0;
            var splitterPos = 0;
            while (cur != (int)';') // collect digits until ;
            {
                cur = ReadNextSymbol();
                if (cur == (int)';') break;
                response[i] = cur - offset;
                i++;
            }
            splitterPos = i;
            i++;
            while (cur != (int)'R') // collect digits until R
            {
                cur = ReadNextSymbol();
                if (cur == (int)'R') break;
                response[i] = cur - offset;
                i++;
            }
            return ProcessCPR(response, splitterPos, i - 1);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private Point ProcessCPR(int[] response, int splitterPos, int endPos)
        {
            int x = 0;
            int y = 0;
            int power = 1;
            for (int i = splitterPos - 1; i >= 0; i--)
            {
                y += response[i] * power;
                power *= 10;
            }
            power = 1;
            for (int i = endPos; i > splitterPos; i--)
            {
                x += response[i] * power;
                power *= 10;
            }
            return new Point(x, y);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private int ReadNextSymbol() => Console.Read();

        public override void SetFullscreen(bool value) =>
            Process.Start("tput", value ? "smcup" : "rmcup").WaitForExit();
    }
}