using Microsoft.Win32;
using System.Reflection;

namespace ChromeOSLauncher.Core
{
    public static class AutoStartManager
    {
        private const string AppName = "ChromeOSLauncher";

        public static void Enable()
        {
            using var key = Registry.CurrentUser.OpenSubKey(
                @"Software\Microsoft\Windows\CurrentVersion\Run", true);

            key?.SetValue(AppName, GetExePath());
        }

        public static void Disable()
        {
            using var key = Registry.CurrentUser.OpenSubKey(
                @"Software\Microsoft\Windows\CurrentVersion\Run", true);

            key?.DeleteValue(AppName, false);
        }

        public static bool IsEnabled()
        {
            using var key = Registry.CurrentUser.OpenSubKey(
                @"Software\Microsoft\Windows\CurrentVersion\Run", false);

            return key?.GetValue(AppName) != null;
        }

        private static string GetExePath()
        {
            return Assembly.GetExecutingAssembly().Location;
        }
    }
}
