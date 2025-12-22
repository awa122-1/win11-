using Microsoft.Win32;
using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;

namespace ChromeOS_Transformer.Core
{
    public class SystemTweaks
    {
        // 1. 修改注册表：让任务栏图标居中 (Win11特性)
        public static void CenterTaskbar()
        {
            try
            {
                using (RegistryKey key = Registry.CurrentUser.OpenSubKey(@"Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", true))
                {
                    if (key != null) key.SetValue("TaskbarAl", 1); // 1=居中, 0=左侧
                }
            }
            catch { /* 忽略权限错误 */ }
        }

        // 2. 调用API：让任务栏全透明
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

        // 3. 设置桌面壁纸
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