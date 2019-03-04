using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using static System.Runtime.InteropServices.RuntimeInformation;

namespace ANSITerm
{
    internal static class Detector
    {
        private const int MaxColors = 13;
        private static int GetMaxColorsFromTermInfo()
        {
            var termInfoDbType = typeof(Console).Assembly.GetType("System.TermInfo+Database", throwOnError: true);
            var readActiveDbMethod = termInfoDbType.GetMethod("ReadActiveDatabase", BindingFlags.NonPublic | BindingFlags.Static);
            var getNumberMethod = termInfoDbType.GetMethod("GetNumber");
            var db = readActiveDbMethod.Invoke(null, null);
            if (db == null)
                throw new IOException("Could not find terminfo database");
            return (int)getNumberMethod.Invoke(db, new object[] { MaxColors });
        }

        public static ColorMode GetBestColorMode()
        {
            if (IsStdOnly())
            {
                return ColorMode.Color16;
            }
            else
            {
                try
                {
                    var colorCount = GetMaxColorsFromTermInfo();
                    return (ColorMode)colorCount;
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine("Could not obtain terminal capabilities from terminfo, assuming 8-color mode");
                    Console.Error.WriteLine("Exception: {0}", ex);
                    return ColorMode.Color8;
                }
            }
        }

        public static bool IsStdOnly()
        {
            // TODO: Add Windows terminal emulator detection like ConEmu and others
            return IsOSPlatform(OSPlatform.Windows);
        }
    }
}