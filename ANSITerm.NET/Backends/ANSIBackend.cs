namespace ANSITerm.Backends
{
    public class ANSIBackend : BackendBase
    {
        internal ANSIBackend()
        {
            ColorMode = ColorMode.Color8;
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

        // TODO: Add additional checks
        public override bool IsColorModeAvailable(ColorMode mode) => true;
    }
}