using System;
using ANSITerm.Backends;

namespace ANSITerm
{
    public static class ANSIConsole
    {
        private static IConsoleBackend s_instance = null;
        public static IConsoleBackend GetInstance()
        {
            // TODO: Add Windows support code
            if (s_instance == null)
                s_instance = new ANSIBackend();
            return s_instance;
        }
    }
}
