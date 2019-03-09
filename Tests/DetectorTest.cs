using System;
using System.Runtime.InteropServices;
using ANSITerm;
using Xunit;

namespace Tests
{
    public class DetectorTest
    {
        [Fact]
        public void ShouldReportMaxColors()
        {
            var colors = (int)Detector.GetBestColorMode();
            Assert.InRange(colors, (int)ColorMode.Color8, (int)ColorMode.TrueColor);
        }
    }
}