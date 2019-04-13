# ANSITerm
Cross-platform .NET Standard 2.0 library for working with terminals.

[API Documentation](https://razer-rbi.github.io/ansiterm-net/api/index.html)

# Installation
```
dotnet add package ANSITerm
```

# Features
* 8, 16, 256-color and true color modes
* Automatic conversion between colors (nearest RGB)
* Uses ANSI sequences if supported
* Supports alternative buffer via `tput`, emulated on Windows cmd
* Platform-independent cursor positioning as defined by ANSI
* Interface is close to `System.Console`, things that don't work on all supported platforms were removed


# Details
Currently there are two backends - `StdBackend` and `ANSIBackend`. First one mostly wraps corefx Console class, second one uses ANSI sequences.

## Windows
Supported terminals (no configuration):
* **Windows CMD** (std) - has maximum of **16 colors**
* **ConEmu** (ANSI) - **true color** support is assumed
* **mintty** (ANSI) - **true color** support is assumed 

Working terminals needing configuration:
* MobaXTerm - `$LINES` and `$ROWS` environment variables must be set, also
`stty -echo -icanon min 1 time 0` should be run prior to launching (use `stty sane` to restore terminal settings).

ANSI backend is used if any of these variables is set:
* `CONEMUANSI=ON` (set by ConEmu)
* `MSYSCON` (set by MSYS2, should contain path to mintty)
* `TERM` (set by MobaXTerm)

## Linux and OS X
ANSI backend is used. Information about color count, keys and other stuff is grabbed from **terminfo** using [corefx TermInfo](https://github.com/dotnet/corefx/blob/v2.2.3/src/System.Console/src/System/TermInfo.cs).

# Example
```csharp
using ANSITerm;

// ...
var term = ANSIConsole.GetInstance();
term.SetFullscreen(true);
term.SetCursorPosition(10, 10);
term.ForegroundColor = new ColorValue(Color.Lime);
term.WriteLine("Hello, world! Press ENTER to exit.");
term.ReadLine();
```

# Can I replace ncurses with it?
Well, no. It's closer to [Python colorama library](https://pypi.org/project/colorama/), but has some additional features.
This project is aimed to be a simple and lightweight, and to contain only core functions that should work on all major platforms.