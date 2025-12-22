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
        private LauncherWindow? _launcher;
        private DispatcherTimer? _daemonTimer;

        public MainWindow()
        {
            InitializeComponent();
            
            // 确保完全隐藏
            this.Visibility = Visibility.Hidden;
            this.ShowInTaskbar = false;

            InitializeSystem();
        }

        private void InitializeSystem()
        {
            // 1. 设置壁纸
            string wallpaperPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Assets", "wallpaper.jpg");
            SystemTweaks.SetWallpaper(wallpaperPath);

            // 2. 居中任务栏
            SystemTweaks.CenterTaskbar();

            // 3. 初始化启动器
            _launcher = new LauncherWindow();
            _launcher.Show(); // 开机先秀一下

            // 4. 定时器：强制保持任务栏透明
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