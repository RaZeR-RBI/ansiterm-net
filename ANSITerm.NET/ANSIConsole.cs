using System;
using ANSITerm.Backends;

namespace ANSITerm
{
    public static class ANSIConsole
    {
        private static IConsoleBackend s_instance = null;
        public static IConsoleBackend GetInstance()
        {
            if (s_instance == null)
                s_instance = DetectAndCreate();
            return s_instance;
        }

        private static IConsoleBackend DetectAndCreate()
        {
            if (Detector.IsStdOnly())
                return new StdBackend();
            return new ANSIBackend();
        }
    }
}
