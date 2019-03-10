using System;
using System.Drawing;
using System.Text;

namespace ANSITerm
{
    /// <summary>
    /// Defines a direction for cursor movement.
    /// 'Forward' equals to 'right' in non-RTL enviroment.
    /// </summary>
    public enum Direction
    {
        Up,
        Down,
        Forward,
        Backward
    }

    /// <summary>
    /// Main interface for console access.
    /// </summary>
    public interface IConsoleBackend
    {
        /// <summary>
        /// Clears the console window.
        /// </summary>
        void Clear();
        /// <summary>
        /// Sets the cursor position.
        /// </summary>
        /// <remarks>
        /// Note: the origin is equal to 0, 0 and is located
        /// on the top-left corner of the terminal part after the
        /// application invocation line, e.g.:
        ///
        /// <code>
        /// user@host:~$ dotnet MyAwesomeApp.dll
        /// Hello, world!
        /// </code>
        /// 
        /// The 'H' symbol is located at 0, 0 in this case, which
        /// should feel pretty familiar to most UNIX terminal users.
        /// On Windows this behaviour is emulated by saving initial
        /// cursor position when the console is initialized and
        /// offsetting by it.
        /// </remarks>
        /// <param name="x">Ranges from 0 to WindowWidth - 1</param>
        /// <param name="y">Ranges from 0 to WindowHeight - 1</param>
        /// <seealso cref="SetCursorPosition(Point)" />
        void SetCursorPosition(int x, int y);
        /// <summary>
        /// Sets the cursor position.
        /// </summary>
        /// <param name="p">Target location</param>
        /// <seealso cref="SetCursorPosition(int, int)" />
        void SetCursorPosition(Point p);
        /// <summary>
        /// Moves the cursor in specified direction by specified count
        /// of steps relative to the current position.
        /// </summary>
        /// <param name="direction">Direction to move the cursor in</param>
        /// <param name="steps">Number of steps (symbols) to move</param>
        void MoveCursor(Direction direction, int steps);
        /// <summary>
        /// Writes a string to the standard output.
        /// </summary>
        /// <seealso cref="WriteLine(string)" />
        void Write(string data);
        /// <summary>
        /// Writes a string to the standard output, terminating it with
        /// <c>Environment.NewLine</c>.
        /// </summary>
        /// <seealso cref="Write(string)" />
        void WriteLine(string data);
        /// <summary>
        /// Writes a string to the standard error stream.
        /// </summary>
        /// <seealso cref="WriteErrorLine(string)" />
        void WriteError(string data);
        /// <summary>
        /// Writes a string to the standard error stream, terminating it
        /// with <c>Environment.NewLine</c>.
        /// </summary>
        /// <seealso cref="WriteError(string)" />
        void WriteErrorLine(string data);
        /// <summary>
        /// Peeks next character from the input stream.
        /// </summary>
        /// <returns>Character value if it exists, -1 otherwise</returns>
        /// <seealso cref="Read()" />
        /// <seealso cref="ReadKey()" />
        int Peek();
        /// <summary>
        /// Consumes the next character from the input stream.
        /// </summary>
        /// <remarks>
        /// This call blocks until a symbol appears in the input
        /// stream.
        /// </remarks>
        /// <seealso cref="Peek()" />
        /// <seealso cref="ReadKey()" />
        int Read();
        /// <summary>
        /// Reads the next character or function key pressed by user. The 
        /// pressed key is displayed.
        /// Equivalent to <see cref="ReadKey(bool)" /> with <c>intercept = true</c>
        /// </summary>
        /// <seealso cref="Peek()" />
        /// <seealso cref="Read()" />
        ConsoleKeyInfo ReadKey();
        /// <summary>
        /// Reads the next character or function key pressed by user.
        /// If <c>intercept</c> equals <c>true</c>, the pressed key is not
        /// displayed in the console window.
        /// </summary>
        /// <seealso cref="Peek()" />
        /// <seealso cref="Read()" />
        /// <seealso cref="ReadKey()" />
        /// <seealso cref="KeyAvailable" />
        ConsoleKeyInfo ReadKey(bool intercept);
        /// <summary>
        /// Reads the next line of characters from the input stream.
        /// This call blocks until the user presses Enter. 
        /// </summary>
        string ReadLine();
        /// <summary>
        /// Indicates if standard input stream is redirected.
        /// </summary>
        /// <returns></returns>
        bool IsInputRedirected { get; }
        /// <summary>
        /// Returns the current column number the cursor is located in.
        /// The first column number is 0.
        /// </summary>
        /// <seealso cref="CursorPosition"/>
        int CursorLeft { get; }
        /// <summary>
        /// Returns the current row number the cursor is located in.
        /// The first row number is 0.
        /// </summary>
        /// <seealso cref="CursorPosition"/>
        int CursorTop { get; }
        /// <summary>
        /// Gets the current row and column numbers indicating current
        /// cursor position.
        /// </summary>
        /// <seealso cref="CursorLeft"/>
        /// <seealso cref="CursorTop"/>
        Point CursorPosition { get; }
        /// <summary>
        /// Toggles the cursor visibility.
        /// </summary>
        bool CursorVisible { set; }
        /// <summary>
        /// Gets or sets the input encoding.
        /// </summary>
        Encoding InputEncoding { get; set; }
        /// <summary>
        /// Indicates if the standard error stream is redirected.
        /// </summary>
        bool IsErrorRedirected { get; }
        /// <summary>
        /// Indicates if the standard output stream is redirected.
        /// </summary>
        bool IsOutputRedirected { get; }
        /// <summary>
        /// Indicates if a keypress is available.
        /// </summary>
        /// <seealso cref="ReadKey()" />
        bool KeyAvailable { get; }
        /// <summary>
        /// Gets or sets the output encoding.
        /// </summary>
        Encoding OutputEncoding { get; set; }
        /// <summary>
        /// Sets the console window title, if possible.
        /// </summary>
        string Title { set; }
        /// <summary>
        /// Gets the terminal window width in columns.
        /// </summary>
        int WindowWidth { get; }
        /// <summary>
        /// Gets the terminal window height in rows.
        /// </summary>
        int WindowHeight { get; }
        /// <summary>
        /// Sets the foreground color. The specified color is converted
        /// to <see cref="ColorMode" /> if needed.
        /// </summary>
        /// <seealso cref="BackgroundColor" />
        /// <seealso cref="ResetColor()" />
        ColorValue ForegroundColor { set; }
        /// <summary>
        /// Sets the background color. The specified color is converted
        /// to <see cref="ColorMode" /> if needed.
        /// </summary>
        /// <seealso cref="ForegroundColor" />
        /// <seealso cref="ResetColor()" />
        ColorValue BackgroundColor { set; }
        /// <summary>
        /// Resets the color values to default values.
        /// </summary>
        void ResetColor();
        /// <summary>
        /// Gets or sets the color mode.
        /// </summary>
        /// <remarks>
        /// If you want to check if a color mode is supported
        /// before setting it, you can use 
        /// <see cref="IsColorModeAvailable(ColorMode)" /> or
        /// <see cref="TrySetColorMode(ColorMode)" /> depending on the usage.
        /// </remarks>
        ColorMode ColorMode { get; set; }
        /// <summary>
        /// Checks if the specified color mode is supported.
        /// </summary>
        /// <remarks>
        /// On platforms where the terminfo database is present,
        /// the maximum color count is taken from it. It may differ with
        /// your actual terminal capabilities, so make sure that your
        /// environment uses proper terminfo definition.
        /// </remarks>
        /// <seealso cref="ColorMode" />
        /// <seealso cref="TrySetColorMode(ColorMode)" />
        bool IsColorModeAvailable(ColorMode mode);
        /// <summary>
        /// Sets the color mode if it's available.
        /// </summary>
        /// <seealso cref="ColorMode" />
        /// <seealso cref="IsColorModeAvailable(ColorMode)" />
        bool TrySetColorMode(ColorMode mode);
        /// <summary>
        /// Toggles the alternative buffer (useful for fullscreen apps).
        /// </summary>
        /// <remarks>
        /// On standard Windows CMD this behaviour is emulated by clearing
        /// the console and setting buffer size equal to window size to disable
        /// scrolling.
        /// </remarks>
        void SetFullscreen(bool value);
    }
}