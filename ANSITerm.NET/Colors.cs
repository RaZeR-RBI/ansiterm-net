using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace ANSITerm
{
    public enum ColorMode : int
    {
        Color8 = 8,
        Color16 = 16,
        Color256 = 256,
        TrueColor = 16777216
    }

    public struct ColorValue
    {
        public int RawValue;
        public ColorMode Mode;

        public static ColorValue FromColor(Color color) =>
            new ColorValue
            {
                RawValue = color.ToArgb(),
                Mode = ColorMode.TrueColor
            };

        public static ColorValue FromIndex(int index, ColorMode mode)
        {
            if (mode == ColorMode.TrueColor)
                throw new InvalidOperationException("Use FromColor for true color mode");
            if (index < 0)
                throw new ArgumentOutOfRangeException("Index cannot be less than zero");
            if (index >= (int)mode)
                throw new ArgumentOutOfRangeException("Index is out of range");
            return new ColorValue
            {
                RawValue = index,
                Mode = mode
            };
        }

        public Color AsColor()
        {
            if (this.Mode == ColorMode.TrueColor)
                return Color.FromArgb(this.RawValue);
            return ColorUtil.IndexedColors[this.RawValue];
        }

        public void Transform(ColorMode targetMode)
        {
            if (this.Mode == targetMode) return;
            if (this.Mode > targetMode)
                Quantize(targetMode);
            else
                Dequantize(targetMode);
        }

        private void Quantize(ColorMode targetMode)
        {
            if (Mode == ColorMode.TrueColor)
            {
                RawValue = ColorUtil.ClosestIndexedTo(Color.FromArgb(RawValue), targetMode);
                Mode = targetMode;
                return;
            }
            RawValue = ColorUtil.QuantizeIndexed(RawValue, targetMode);
            Mode = targetMode;
        }

        private void Dequantize(ColorMode targetMode)
        {
            if (targetMode == ColorMode.TrueColor)
                RawValue = ColorUtil.IndexedColors[this.RawValue].ToArgb();
            Mode = targetMode;
        }
    }

    public enum Color8 : byte
    {
        Black = 0,
        Red,
        Green,
        Yellow,
        Blue,
        Magenta,
        Cyan,
        White
    }

    public enum Color16 : byte
    {
        Black = 0,
        Red,
        Green,
        Yellow,
        Blue,
        Magenta,
        Cyan,
        White,
        BrightBlack,
        BrightRed,
        BrightGreen,
        BrightYellow,
        BrightBlue,
        BrightMagenta,
        BrightCyan,
        BrightWhite
    }

    public enum Color256
    {
        /* First 8 colors as in 8 color mode */
        Black = 0,
        Red,
        Green,
        Yellow,
        Blue,
        Magenta,
        Cyan,
        White,
        /* Next 8 colors from 16 color mode */
        BrightBlack,
        BrightRed,
        BrightGreen,
        BrightYellow,
        BrightBlue,
        BrightMagenta,
        BrightCyan,
        BrightWhite,
        /* Rest is from the 256 color mode */
        Grey16,
        NavyBlue17,
        DarkBlue18,
        Blue19,
        Blue20,
        Blue21,
        DarkGreen22,
        DeepSkyBlue23,
        DeepSkyBlue24,
        DeepSkyBlue25,
        DodgerBlue26,
        DodgerBlue27,
        Green28,
        SpringGreen29,
        Turquoise30,
        DeepSkyBlue31,
        DeepSkyBlue32,
        DodgerBlue33,
        Green34,
        SpringGreen35,
        DarkCyan36,
        LightSeaGreen37,
        DeepSkyBlue38,
        DeepSkyBlue39,
        Green40,
        SpringGreen41,
        SpringGreen42,
        Cyan43,
        DarkTurquoise44,
        Turquoise45,
        Green46,
        SpringGreen47,
        SpringGreen48,
        MediumSpringGreen49,
        Cyan50,
        Cyan51,
        DarkRed52,
        DeepPink53,
        Purple54,
        Purple55,
        Purple56,
        BlueViolet57,
        Orange58,
        Grey59,
        MediumPurple60,
        SlateBlue61,
        SlateBlue62,
        RoyalBlue63,
        Chartreuse64,
        DarkSeaGreen65,
        PaleTurquoise66,
        SteelBlue67,
        SteelBlue68,
        CornflowerBlue69,
        Chartreuse70,
        DarkSeaGreen71,
        CadetBlue72,
        CadetBlue73,
        SkyBlue74,
        SteelBlue75,
        Chartreuse76,
        PaleGreen77,
        SeaGreen78,
        Aquamarine79,
        MediumTurquoise80,
        SteelBlue81,
        Chartreuse82,
        SeaGreen83,
        SeaGreen84,
        SeaGreen85,
        Aquamarine86,
        DarkSlateGray87,
        DarkRed88,
        DeepPink89,
        DarkMagenta90,
        DarkMagenta91,
        DarkViolet92,
        Purple93,
        Orange94,
        LightPink95,
        Plum96,
        MediumPurple97,
        MediumPurple98,
        SlateBlue99,
        Yellow100,
        Wheat101,
        Grey102,
        LightSlateGrey103,
        MediumPurple104,
        LightSlateBlue105,
        Yellow106,
        DarkOliveGreen107,
        DarkSeaGreen108,
        LightSkyBlue109,
        LightSkyBlue110,
        SkyBlue111,
        Chartreuse112,
        DarkOliveGreen113,
        PaleGreen114,
        DarkSeaGreen115,
        DarkSlateGray116,
        SkyBlue117,
        Chartreuse118,
        LightGreen119,
        LightGreen120,
        PaleGreen121,
        Aquamarine122,
        DarkSlateGray123,
        Red124,
        DeepPink125,
        MediumVioletRed126,
        Magenta127,
        DarkViolet128,
        Purple129,
        DarkOrange130,
        IndianRed131,
        HotPink132,
        MediumOrchid133,
        MediumOrchid134,
        MediumPurple135,
        DarkGoldenrod136,
        LightSalmon137,
        RosyBrown138,
        Grey139,
        MediumPurple140,
        MediumPurple141,
        Gold142,
        DarkKhaki143,
        NavajoWhite144,
        Grey145,
        LightSteelBlue146,
        LightSteelBlue147,
        Yellow148,
        DarkOliveGreen149,
        DarkSeaGreen150,
        DarkSeaGreen151,
        LightCyan152,
        LightSkyBlue153,
        GreenYellow154,
        DarkOliveGreen155,
        PaleGreen156,
        DarkSeaGreen157,
        DarkSeaGreen158,
        PaleTurquoise159,
        Red160,
        DeepPink161,
        DeepPink162,
        Magenta163,
        Magenta164,
        Magenta165,
        DarkOrange166,
        IndianRed167,
        HotPink168,
        HotPink169,
        Orchid170,
        MediumOrchid171,
        Orange172,
        LightSalmon173,
        LightPink174,
        Pink175,
        Plum176,
        Violet177,
        Gold178,
        LightGoldenrod179,
        Tan180,
        MistyRose181,
        Thistle182,
        Plum183,
        Yellow184,
        Khaki185,
        LightGoldenrod186,
        LightYellow187,
        Grey188,
        LightSteelBlue189,
        Yellow190,
        DarkOliveGreen191,
        DarkOliveGreen192,
        DarkSeaGreen193,
        Honeydew194,
        LightCyan195,
        Red196,
        DeepPink197,
        DeepPink198,
        DeepPink199,
        Magenta200,
        Magenta201,
        OrangeRed202,
        IndianRed203,
        IndianRed204,
        HotPink205,
        HotPink206,
        MediumOrchid207,
        DarkOrange208,
        Salmon209,
        LightCoral210,
        PaleVioletRed211,
        Orchid212,
        Orchid213,
        Orange214,
        SandyBrown215,
        LightSalmon216,
        LightPink217,
        Pink218,
        Plum219,
        Gold220,
        LightGoldenrod221,
        LightGoldenrod222,
        NavajoWhite223,
        MistyRose224,
        Thistle225,
        Yellow226,
        LightGoldenrod227,
        Khaki228,
        Wheat229,
        Cornsilk230,
        Grey231,
        Grey232,
        Grey233,
        Grey234,
        Grey235,
        Grey236,
        Grey237,
        Grey238,
        Grey239,
        Grey240,
        Grey241,
        Grey242,
        Grey243,
        Grey244,
        Grey245,
        Grey246,
        Grey247,
        Grey248,
        Grey249,
        Grey250,
        Grey251,
        Grey252,
        Grey253,
        Grey254,
        Grey255,

    }


    internal static class ColorUtil
    {
        public static IReadOnlyList<Color> IndexedColors => s_indexedColors;

        public static float DistanceSquared(Color one, Color two)
        {
            // https://en.wikipedia.org/wiki/Color_difference#Euclidean
            float r = (one.R + two.R) / 2.0f;
            float deltaRSq = one.R - two.R; deltaRSq *= deltaRSq;
            float deltaGSq = one.G - two.G; deltaGSq *= deltaGSq;
            float deltaBSq = one.B - two.B; deltaBSq *= deltaBSq;
            return (2 + r / 256.0f) * deltaRSq +
                4 * deltaGSq +
                (2 + (255.0f - r) / 256.0f) * deltaBSq;
        }

        private static Dictionary<ColorMode, byte[]> s_indexMaps =
            new Dictionary<ColorMode, byte[]>();

        public static int QuantizeIndexed(int index, ColorMode targetMode)
        {
            return s_indexMaps[targetMode][index];
        }

        static ColorUtil()
        {
            var modes = new ColorMode[] { ColorMode.Color8, ColorMode.Color16 };
            foreach (var mode in modes)
            {
                var count = (int)mode;
                var indexes = new byte[IndexedColors.Count];
                for (byte j = 0; j < count; j++)
                {
                    indexes[j] = j;
                }
                for (var i = count; i < indexes.Length; i++)
                {
                    // map bright colors to normal ones
                    if (mode == ColorMode.Color8 && i < 16)
                    {
                        indexes[i] = (byte)(i - 8); continue;
                    }
                    indexes[i] = (byte)ClosestIndexedTo(IndexedColors[i], mode);
                }
                s_indexMaps.Add(mode, indexes);
            }
        }

        public static int ClosestIndexedTo(Color target, ColorMode mode)
        {
            var distances = new float[(int)mode];
            for (var i = 0; i < distances.Length; i++)
            {
                distances[i] = DistanceSquared(target, IndexedColors[i]);
                if (distances[i] <= float.Epsilon)
                {
                    return i;
                }
            }
            return Array.IndexOf(distances, distances.Min());
        }

        private static Color[] s_indexedColors = new Color[] {
            Color.FromArgb(0, 0, 0),
            Color.FromArgb(128, 0, 0),
            Color.FromArgb(0, 128, 0),
            Color.FromArgb(128, 128, 0),
            Color.FromArgb(0, 0, 128),
            Color.FromArgb(128, 0, 128),
            Color.FromArgb(0, 128, 128),
            Color.FromArgb(192, 192, 192),
            Color.FromArgb(128, 128, 128),
            Color.FromArgb(255, 0, 0),
            Color.FromArgb(0, 255, 0),
            Color.FromArgb(255, 255, 0),
            Color.FromArgb(0, 0, 255),
            Color.FromArgb(255, 0, 255),
            Color.FromArgb(0, 255, 255),
            Color.FromArgb(255, 255, 255),
            Color.FromArgb(0, 0, 0),
            Color.FromArgb(0, 0, 95),
            Color.FromArgb(0, 0, 135),
            Color.FromArgb(0, 0, 175),
            Color.FromArgb(0, 0, 215),
            Color.FromArgb(0, 0, 255),
            Color.FromArgb(0, 95, 0),
            Color.FromArgb(0, 95, 95),
            Color.FromArgb(0, 95, 135),
            Color.FromArgb(0, 95, 175),
            Color.FromArgb(0, 95, 215),
            Color.FromArgb(0, 95, 255),
            Color.FromArgb(0, 135, 0),
            Color.FromArgb(0, 135, 95),
            Color.FromArgb(0, 135, 135),
            Color.FromArgb(0, 135, 175),
            Color.FromArgb(0, 135, 215),
            Color.FromArgb(0, 135, 255),
            Color.FromArgb(0, 175, 0),
            Color.FromArgb(0, 175, 95),
            Color.FromArgb(0, 175, 135),
            Color.FromArgb(0, 175, 175),
            Color.FromArgb(0, 175, 215),
            Color.FromArgb(0, 175, 255),
            Color.FromArgb(0, 215, 0),
            Color.FromArgb(0, 215, 95),
            Color.FromArgb(0, 215, 135),
            Color.FromArgb(0, 215, 175),
            Color.FromArgb(0, 215, 215),
            Color.FromArgb(0, 215, 255),
            Color.FromArgb(0, 255, 0),
            Color.FromArgb(0, 255, 95),
            Color.FromArgb(0, 255, 135),
            Color.FromArgb(0, 255, 175),
            Color.FromArgb(0, 255, 215),
            Color.FromArgb(0, 255, 255),
            Color.FromArgb(95, 0, 0),
            Color.FromArgb(95, 0, 95),
            Color.FromArgb(95, 0, 135),
            Color.FromArgb(95, 0, 175),
            Color.FromArgb(95, 0, 215),
            Color.FromArgb(95, 0, 255),
            Color.FromArgb(95, 95, 0),
            Color.FromArgb(95, 95, 95),
            Color.FromArgb(95, 95, 135),
            Color.FromArgb(95, 95, 175),
            Color.FromArgb(95, 95, 215),
            Color.FromArgb(95, 95, 255),
            Color.FromArgb(95, 135, 0),
            Color.FromArgb(95, 135, 95),
            Color.FromArgb(95, 135, 135),
            Color.FromArgb(95, 135, 175),
            Color.FromArgb(95, 135, 215),
            Color.FromArgb(95, 135, 255),
            Color.FromArgb(95, 175, 0),
            Color.FromArgb(95, 175, 95),
            Color.FromArgb(95, 175, 135),
            Color.FromArgb(95, 175, 175),
            Color.FromArgb(95, 175, 215),
            Color.FromArgb(95, 175, 255),
            Color.FromArgb(95, 215, 0),
            Color.FromArgb(95, 215, 95),
            Color.FromArgb(95, 215, 135),
            Color.FromArgb(95, 215, 175),
            Color.FromArgb(95, 215, 215),
            Color.FromArgb(95, 215, 255),
            Color.FromArgb(95, 255, 0),
            Color.FromArgb(95, 255, 95),
            Color.FromArgb(95, 255, 135),
            Color.FromArgb(95, 255, 175),
            Color.FromArgb(95, 255, 215),
            Color.FromArgb(95, 255, 255),
            Color.FromArgb(135, 0, 0),
            Color.FromArgb(135, 0, 95),
            Color.FromArgb(135, 0, 135),
            Color.FromArgb(135, 0, 175),
            Color.FromArgb(135, 0, 215),
            Color.FromArgb(135, 0, 255),
            Color.FromArgb(135, 95, 0),
            Color.FromArgb(135, 95, 95),
            Color.FromArgb(135, 95, 135),
            Color.FromArgb(135, 95, 175),
            Color.FromArgb(135, 95, 215),
            Color.FromArgb(135, 95, 255),
            Color.FromArgb(135, 135, 0),
            Color.FromArgb(135, 135, 95),
            Color.FromArgb(135, 135, 135),
            Color.FromArgb(135, 135, 175),
            Color.FromArgb(135, 135, 215),
            Color.FromArgb(135, 135, 255),
            Color.FromArgb(135, 175, 0),
            Color.FromArgb(135, 175, 95),
            Color.FromArgb(135, 175, 135),
            Color.FromArgb(135, 175, 175),
            Color.FromArgb(135, 175, 215),
            Color.FromArgb(135, 175, 255),
            Color.FromArgb(135, 215, 0),
            Color.FromArgb(135, 215, 95),
            Color.FromArgb(135, 215, 135),
            Color.FromArgb(135, 215, 175),
            Color.FromArgb(135, 215, 215),
            Color.FromArgb(135, 215, 255),
            Color.FromArgb(135, 255, 0),
            Color.FromArgb(135, 255, 95),
            Color.FromArgb(135, 255, 135),
            Color.FromArgb(135, 255, 175),
            Color.FromArgb(135, 255, 215),
            Color.FromArgb(135, 255, 255),
            Color.FromArgb(175, 0, 0),
            Color.FromArgb(175, 0, 95),
            Color.FromArgb(175, 0, 135),
            Color.FromArgb(175, 0, 175),
            Color.FromArgb(175, 0, 215),
            Color.FromArgb(175, 0, 255),
            Color.FromArgb(175, 95, 0),
            Color.FromArgb(175, 95, 95),
            Color.FromArgb(175, 95, 135),
            Color.FromArgb(175, 95, 175),
            Color.FromArgb(175, 95, 215),
            Color.FromArgb(175, 95, 255),
            Color.FromArgb(175, 135, 0),
            Color.FromArgb(175, 135, 95),
            Color.FromArgb(175, 135, 135),
            Color.FromArgb(175, 135, 175),
            Color.FromArgb(175, 135, 215),
            Color.FromArgb(175, 135, 255),
            Color.FromArgb(175, 175, 0),
            Color.FromArgb(175, 175, 95),
            Color.FromArgb(175, 175, 135),
            Color.FromArgb(175, 175, 175),
            Color.FromArgb(175, 175, 215),
            Color.FromArgb(175, 175, 255),
            Color.FromArgb(175, 215, 0),
            Color.FromArgb(175, 215, 95),
            Color.FromArgb(175, 215, 135),
            Color.FromArgb(175, 215, 175),
            Color.FromArgb(175, 215, 215),
            Color.FromArgb(175, 215, 255),
            Color.FromArgb(175, 255, 0),
            Color.FromArgb(175, 255, 95),
            Color.FromArgb(175, 255, 135),
            Color.FromArgb(175, 255, 175),
            Color.FromArgb(175, 255, 215),
            Color.FromArgb(175, 255, 255),
            Color.FromArgb(215, 0, 0),
            Color.FromArgb(215, 0, 95),
            Color.FromArgb(215, 0, 135),
            Color.FromArgb(215, 0, 175),
            Color.FromArgb(215, 0, 215),
            Color.FromArgb(215, 0, 255),
            Color.FromArgb(215, 95, 0),
            Color.FromArgb(215, 95, 95),
            Color.FromArgb(215, 95, 135),
            Color.FromArgb(215, 95, 175),
            Color.FromArgb(215, 95, 215),
            Color.FromArgb(215, 95, 255),
            Color.FromArgb(215, 135, 0),
            Color.FromArgb(215, 135, 95),
            Color.FromArgb(215, 135, 135),
            Color.FromArgb(215, 135, 175),
            Color.FromArgb(215, 135, 215),
            Color.FromArgb(215, 135, 255),
            Color.FromArgb(215, 175, 0),
            Color.FromArgb(215, 175, 95),
            Color.FromArgb(215, 175, 135),
            Color.FromArgb(215, 175, 175),
            Color.FromArgb(215, 175, 215),
            Color.FromArgb(215, 175, 255),
            Color.FromArgb(215, 215, 0),
            Color.FromArgb(215, 215, 95),
            Color.FromArgb(215, 215, 135),
            Color.FromArgb(215, 215, 175),
            Color.FromArgb(215, 215, 215),
            Color.FromArgb(215, 215, 255),
            Color.FromArgb(215, 255, 0),
            Color.FromArgb(215, 255, 95),
            Color.FromArgb(215, 255, 135),
            Color.FromArgb(215, 255, 175),
            Color.FromArgb(215, 255, 215),
            Color.FromArgb(215, 255, 255),
            Color.FromArgb(255, 0, 0),
            Color.FromArgb(255, 0, 95),
            Color.FromArgb(255, 0, 135),
            Color.FromArgb(255, 0, 175),
            Color.FromArgb(255, 0, 215),
            Color.FromArgb(255, 0, 255),
            Color.FromArgb(255, 95, 0),
            Color.FromArgb(255, 95, 95),
            Color.FromArgb(255, 95, 135),
            Color.FromArgb(255, 95, 175),
            Color.FromArgb(255, 95, 215),
            Color.FromArgb(255, 95, 255),
            Color.FromArgb(255, 135, 0),
            Color.FromArgb(255, 135, 95),
            Color.FromArgb(255, 135, 135),
            Color.FromArgb(255, 135, 175),
            Color.FromArgb(255, 135, 215),
            Color.FromArgb(255, 135, 255),
            Color.FromArgb(255, 175, 0),
            Color.FromArgb(255, 175, 95),
            Color.FromArgb(255, 175, 135),
            Color.FromArgb(255, 175, 175),
            Color.FromArgb(255, 175, 215),
            Color.FromArgb(255, 175, 255),
            Color.FromArgb(255, 215, 0),
            Color.FromArgb(255, 215, 95),
            Color.FromArgb(255, 215, 135),
            Color.FromArgb(255, 215, 175),
            Color.FromArgb(255, 215, 215),
            Color.FromArgb(255, 215, 255),
            Color.FromArgb(255, 255, 0),
            Color.FromArgb(255, 255, 95),
            Color.FromArgb(255, 255, 135),
            Color.FromArgb(255, 255, 175),
            Color.FromArgb(255, 255, 215),
            Color.FromArgb(255, 255, 255),
            Color.FromArgb(8, 8, 8),
            Color.FromArgb(18, 18, 18),
            Color.FromArgb(28, 28, 28),
            Color.FromArgb(38, 38, 38),
            Color.FromArgb(48, 48, 48),
            Color.FromArgb(58, 58, 58),
            Color.FromArgb(68, 68, 68),
            Color.FromArgb(78, 78, 78),
            Color.FromArgb(88, 88, 88),
            Color.FromArgb(98, 98, 98),
            Color.FromArgb(108, 108, 108),
            Color.FromArgb(118, 118, 118),
            Color.FromArgb(128, 128, 128),
            Color.FromArgb(138, 138, 138),
            Color.FromArgb(148, 148, 148),
            Color.FromArgb(158, 158, 158),
            Color.FromArgb(168, 168, 168),
            Color.FromArgb(178, 178, 178),
            Color.FromArgb(188, 188, 188),
            Color.FromArgb(198, 198, 198),
            Color.FromArgb(208, 208, 208),
            Color.FromArgb(218, 218, 218),
            Color.FromArgb(228, 228, 228),
            Color.FromArgb(238, 238, 238)
        };
    }
    // TODO: Add required enums and constants
}