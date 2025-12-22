using ChromeOS_Transformer.Core;
using ChromeOS_Transformer.UI;
using System;
using System.IO;
using System.Windows;
using System.Windows.Threading;

namespace ChromeOS_Transformer
{
    public partial class MainWindow : Window
    {
        private LauncherWindow _launcher;
        private DispatcherTimer _daemonTimer;

        public MainWindow()
        {
            InitializeComponent();
            
            // 1. 隐藏主窗口 (我们只需要它在后台干活)
            this.Visibility = Visibility.Hidden;
            this.ShowInTaskbar = false;

            // 2. 初始化系统改造
            InitializeSystem();

            // 3. 准备启动器
            _launcher = new LauncherWindow();
            _launcher.Show(); // 启动时自动展示一下
        }

        private void InitializeSystem()
        {
            // A. 设置壁纸
            string wallpaperPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Assets", "wallpaper.jpg");
            if (File.Exists(wallpaperPath))
            {
                SystemTweaks.SetWallpaper(wallpaperPath);
            }

            // B. 任务栏居中
            SystemTweaks.CenterTaskbar();

            // C. 启动守护定时器 (强制任务栏保持透明)
            // Windows 有时会重绘任务栏导致透明失效，所以我们需要每隔几秒“刷新”一次
            _daemonTimer = new DispatcherTimer();
            _daemonTimer.Interval = TimeSpan.FromSeconds(2);
            _daemonTimer.Tick += (s, e) => 
            {
                SystemTweaks.SetTaskbarTransparent();
            };
            _daemonTimer.Start();
        }
    }
}