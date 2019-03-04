using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using ANSITerm;

namespace Sample
{
    class Program
    {
        static string block = "-";

        static void Main(string[] args)
        {
            var term = ANSIConsole.GetInstance();
            var parts = new Dictionary<ColorMode, Action>() {
                {ColorMode.Color8, () => Print8ColorPalette(term) },
                {ColorMode.Color16, () => Print16ColorPalette(term) },
                {ColorMode.Color256, () => Print256ColorPalette(term) },
                {ColorMode.TrueColor, () => PrintTrueColor(term) }
            };
            foreach (var pair in parts)
            {
                var available = term.TrySetColorMode(pair.Key);
                term.WriteLine($"Mode: {pair.Key}, available: {available}");
                pair.Value();
                term.ResetColor();
                term.Write("\n\n\n");
            }
        }


        static void Print8ColorPalette(IConsoleBackend term)
        {
            foreach (var color in EnumValues<Color8>())
                Display(term, color);
        }

        static void Print16ColorPalette(IConsoleBackend term)
        {
            foreach (var color in EnumValues<Color16>())
                Display(term, color);
        }

        static void Print256ColorPalette(IConsoleBackend term)
        {
            var i = 0;
            for (i = 0; i < 16; i++)
                Display(term, (Color256)i);
            term.Write("\n\n");

            for (i = 16; i < 232; i++)
            {
                Display(term, (Color256)i);
                if ((i - 16) > 0 && (i - 15) % 36 == 0)
                    term.Write("\n");
            }
            term.ResetColor();
            term.Write("\n");
            for (i = 232; i < 256; i++)
                Display(term, (Color256)i);
        }

        static void PrintTrueColor(IConsoleBackend term)
        {
            var columns = term.WindowWidth / 2;
            var hueStep = 360.0 / columns;
            for (var i = 0; i < columns; i++)
            {
                var color = ColorFromHSV(i * hueStep, 1.0, 0.5);
                Display(term, color);
            }
        }

        static IEnumerable<T> EnumValues<T>() =>
            Enum.GetValues(typeof(T)).Cast<T>();

        static void Display(IConsoleBackend term, Color8 color)
        {
            term.ForegroundColor = new ColorValue(color);
            term.Write(block);
            term.ResetColor();
            term.BackgroundColor = new ColorValue(color);
            term.Write(block);
            term.ResetColor();
        }

        static void Display(IConsoleBackend term, Color16 color)
        {
            term.ForegroundColor = new ColorValue(color);
            term.Write(block);
            term.ResetColor();
            term.BackgroundColor = new ColorValue(color);
            term.Write(block);
            term.ResetColor();
        }

        static void Display(IConsoleBackend term, Color256 color)
        {
            term.ForegroundColor = new ColorValue(color);
            term.Write(block);
            term.ResetColor();
            term.BackgroundColor = new ColorValue(color);
            term.Write(block);
            term.ResetColor();
        }

        static void Display(IConsoleBackend term, Color color)
        {
            term.ForegroundColor = new ColorValue(color);
            term.Write(block);
            term.ResetColor();
            term.BackgroundColor = new ColorValue(color);
            term.Write(block);
            term.ResetColor();
        }

        public static Color ColorFromHSV(double hue, double saturation, double value)
        {
            int hi = Convert.ToInt32(Math.Floor(hue / 60)) % 6;
            double f = hue / 60 - Math.Floor(hue / 60);

            value = value * 255;
            int v = Convert.ToInt32(value);
            int p = Convert.ToInt32(value * (1 - saturation));
            int q = Convert.ToInt32(value * (1 - f * saturation));
            int t = Convert.ToInt32(value * (1 - (1 - f) * saturation));

            if (hi == 0)
                return Color.FromArgb(255, v, t, p);
            else if (hi == 1)
                return Color.FromArgb(255, q, v, p);
            else if (hi == 2)
                return Color.FromArgb(255, p, v, t);
            else if (hi == 3)
                return Color.FromArgb(255, p, q, v);
            else if (hi == 4)
                return Color.FromArgb(255, t, p, v);
            else
                return Color.FromArgb(255, v, p, q);
        }
    }
}
