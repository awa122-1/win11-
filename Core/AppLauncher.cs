using System.Diagnostics;

namespace ChromeOSLauncher.Core
{
    public static class AppLauncher
    {
        public static void LaunchExe(string path)
        {
            Process.Start(new ProcessStartInfo
            {
                FileName = path,
                UseShellExecute = true
            });
        }

        public static void LaunchMsi(string path)
        {
            Process.Start(new ProcessStartInfo
            {
                FileName = "msiexec",
                Arguments = $"/i \"{path}\"",
                UseShellExecute = true,
                Verb = "runas"
            });
        }
    }
}
