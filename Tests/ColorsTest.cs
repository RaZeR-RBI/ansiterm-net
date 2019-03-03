using System;
using System.Linq;
using ANSITerm;
using Xunit;
using FluentAssertions;
using System.Drawing;
using System.Collections.Generic;
using System.Collections;

namespace Tests
{
    public class ColorsTest
    {
        [Theory]
        [InlineData(ColorMode.Color8)]
        [InlineData(ColorMode.Color16)]
        public void EnsureIndexTableSanity(ColorMode mode)
        {
            var maxColors = (int)mode;
            var indexes = Enumerable.Range(0, 256)
                .Select(i => ColorUtil.QuantizeIndexed(i, mode))
                .ToList();

            var distinctIndexes = indexes.Distinct().OrderBy(i => i).ToList();
            Assert.True(distinctIndexes.Count == maxColors);
            distinctIndexes.Should().OnlyContain(i => i >= 0 && i < maxColors);

            var equalColors = indexes.Take(maxColors);
            equalColors.Should().BeEquivalentTo(Enumerable.Range(0, maxColors));
        }

        [Theory]
        [ClassData(typeof(ColorConversionTestData))]
        public void TestColorConversion(Color source, int index, ColorMode mode)
        {
            var color = new ColorValue(source);
            Assert.Equal(source.ToArgb(), color.RawValue);
            Assert.Equal(ColorMode.TrueColor, color.Mode);

            color.Transform(mode);
            Assert.Equal(index, color.RawValue);
            Assert.Equal(mode, color.Mode);
        }
    }

    public class ColorConversionTestData : IEnumerable<object[]>
    {
        public IEnumerator<object[]> GetEnumerator()
        {
            yield return new object[] { Color.Red, 1, ColorMode.Color8 };
            yield return new object[] { Color.Red, 9, ColorMode.Color16 };
            yield return new object[] { Color.FromArgb(0xFACADE), 224, ColorMode.Color256 };
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
