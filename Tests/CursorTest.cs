using Xunit;
using FluentAssertions;
using ANSITerm;

namespace Tests
{
    public class CursorTest
    {
        [Fact]
        public void ShouldGetAndSetCursorPosition()
        {
            var term = ANSIConsole.GetInstance();
            var position = term.CursorPosition;
            term.SetCursorPosition(position);
            term.CursorPosition.Should().BeEquivalentTo(position);
        }
    }
}