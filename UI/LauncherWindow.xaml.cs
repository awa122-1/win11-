using ChromeOS_Transformer.Core;
using ChromeOS_Transformer.UI;
using System;
using System.IO;
using System.Net.Http; // 用于下载壁纸
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;

namespace ChromeOS_Transformer
{
    public partial class MainWindow : Window
    {
        private LauncherWindow? _launcher;
        private DispatcherTimer? _daemonTimer;

        public MainWindow()
        {
            InitializeComponent();
            this.Visibility = Visibility.Hidden;
            this.ShowInTaskbar = false;

            InitializeSystem();
        }

        private async void InitializeSystem()
        {
            // 1. 在线下载壁纸并设置 (无需本地文件)
            await SetOnlineWallpaper();

            // 2. 居中任务栏
            SystemTweaks.CenterTaskbar();

            // 3. 启动器
            _launcher = new LauncherWindow();
            _launcher.Show();

            // 4. 保持透明
            _daemonTimer = new DispatcherTimer();
            _daemonTimer.Interval = TimeSpan.FromSeconds(2);
            _daemonTimer.Tick += (s, e) => SystemTweaks.SetTaskbarTransparent();
            _daemonTimer.Start();
        }

        private async Task SetOnlineWallpaper()
        {
            string tempPath = Path.Combine(Path.GetTempPath(), "chromeos_bg.jpg");
            
            // 如果临时文件夹里已经有下载过的壁纸，就直接用，不重复下载
            if (!File.Exists(tempPath))
            {
                try
                {
                    using (var client = new HttpClient())
                    {
                        // 这里使用一张高质量的 Material Design 壁纸链接
                        var imageUrl = "https://images.unsplash.com/photo-1557683316-973673baf926?q=80&w=2029&auto=format&fit=crop";
                        var data = await client.GetByteArrayAsync(imageUrl);
                        await File.WriteAllBytesAsync(tempPath, data);
                    }
                }
                catch { /* 下载失败就忽略，不换壁纸 */ }
            }

            if (File.Exists(tempPath))
            {
                SystemTweaks.SetWallpaper(tempPath);
            }
        }
    }
}