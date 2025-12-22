using Microsoft.Win32;
using System;
using System.IO;
using System.Runtime.InteropServices;

namespace ChromeOS_Transformer.Core
{
    public class SystemTweaks
    {
        public static void CenterTaskbar()
        {
            try
            {
                using (RegistryKey key = Registry.CurrentUser.OpenSubKey(@"Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", true))
                {
                    if (key != null) key.SetValue("TaskbarAl", 1);
                }
            }
            catch { }
        }

        public static void SetTaskbarTransparent()
        {
            IntPtr taskbarHwnd = NativeMethods.FindWindow("Shell_TrayWnd", null);
            if (taskbarHwnd == IntPtr.Zero) return;

            var accent = new NativeMethods.AccentPolicy
            {
                AccentState = NativeMethods.AccentState.ACCENT_ENABLE_TRANSPARENTGRADIENT,
                GradientColor = 0
            };

            var accentStructSize = Marshal.SizeOf(accent);
            var accentPtr = Marshal.AllocHGlobal(accentStructSize);
            Marshal.StructureToPtr(accent, accentPtr, false);

            var data = new NativeMethods.WindowCompositionAttributeData
            {
                Attribute = NativeMethods.WindowCompositionAttribute.WCA_ACCENT_POLICY,
                SizeOfData = accentStructSize,
                Data = accentPtr
            };

            NativeMethods.SetWindowCompositionAttribute(taskbarHwnd, ref data);
            Marshal.FreeHGlobal(accentPtr);
        }

        public static void SetWallpaper(string path)
        {
            if (File.Exists(path))
            {
                NativeMethods.SystemParametersInfo(NativeMethods.SPI_SETDESKWALLPAPER, 0, path,
                    NativeMethods.SPIF_UPDATEINIFILE | NativeMethods.SPIF_SENDWININICHANGE);
            }
        }
    }
}
// ... 之前的代码保持不变 ...

        // [新增] 还原任务栏样式 (取消透明)
        public static void RestoreTaskbar()
        {
            IntPtr taskbarHwnd = NativeMethods.FindWindow("Shell_TrayWnd", null);
            if (taskbarHwnd == IntPtr.Zero) return;

            var accent = new NativeMethods.AccentPolicy
            {
                // 设置为 DISABLED 即可恢复系统默认
                AccentState = NativeMethods.AccentState.ACCENT_DISABLED, 
                GradientColor = 0
            };

            var accentStructSize = Marshal.SizeOf(accent);
            var accentPtr = Marshal.AllocHGlobal(accentStructSize);
            Marshal.StructureToPtr(accent, accentPtr, false);

            var data = new NativeMethods.WindowCompositionAttributeData
            {
                Attribute = NativeMethods.WindowCompositionAttribute.WCA_ACCENT_POLICY,
                SizeOfData = accentStructSize,
                Data = accentPtr
            };

            NativeMethods.SetWindowCompositionAttribute(taskbarHwnd, ref data);
            Marshal.FreeHGlobal(accentPtr);
        }