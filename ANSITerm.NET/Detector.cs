using System;
using System.Diagnostics;
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
                    return GetANSIColorCount();
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
            if (!IsOSPlatform(OSPlatform.Windows)) return false;
            // Windows checks
            return !(IsConEmuANSI() || IsMSYS2() || HasTermSet());
        }

        private static bool IsConEmuANSI() =>
            Environment.GetEnvironmentVariable("CONEMUANSI") == "ON";
        
        private static bool IsMSYS2() =>
            Environment.GetEnvironmentVariable("MSYSCON") != null;
        
        private static bool HasTermSet() =>
            Environment.GetEnvironmentVariable("TERM") != null;

        private static ColorMode GetANSIColorCount()
        {
            if (IsOSPlatform(OSPlatform.Windows))
            // assume that true color support is enabled
            // looks like there is no way to properly detect it
                return ColorMode.TrueColor;
            
            var colors = GetMaxColorsFromTermInfo();
            var result = ColorMode.Color8;
            foreach(var mode in Enum.GetValues(typeof(ColorMode)).Cast<int>())
            {
                if (colors < mode) break;
                result = (ColorMode)mode;
            }
            return result;
        }

        public static void Setup()
        {
            if (IsMSYS2())
                TrySetSttyParameters();
        }

        private static void TrySetSttyParameters()
        {
            try
            {
                Process.Start("stty", "-icanon min 1 time 0").WaitForExit();
            } catch (Exception) {}
        }
    }
}