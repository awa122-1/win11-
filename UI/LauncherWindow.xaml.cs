using System;
using System.Diagnostics;
using System.IO;
using System.Media;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace ChromeOS_Transformer.UI
{
    public partial class LauncherWindow : Window
    {
        private SoundPlayer _clickSound;
        private string _soundPath;

        public LauncherWindow()
        {
            InitializeComponent();
            InitSound();
        }

        private void InitSound()
        {
            _soundPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Assets", "click.wav");
            if (File.Exists(_soundPath))
            {
                _clickSound = new SoundPlayer(_soundPath);
                try { _clickSound.Load(); } catch { }
            }
        }

        private void PlaySound()
        {
            try { _clickSound?.Play(); } catch { }
        }

        // 处理APP点击
        private void App_Click(object sender, RoutedEventArgs e)
        {
            PlaySound(); // 1. 播放音效

            Button btn = sender as Button;
            string tag = btn.Tag.ToString();

            try
            {
                ProcessStartInfo psi = new ProcessStartInfo();
                psi.UseShellExecute = true; // .NET Core 必须开启这个才能打开URL

                switch (tag)
                {
                    case "chrome":
                        psi.FileName = "chrome.exe";
                        break;
                    case "youtube":
                        // 尝试以 Chrome APP 模式打开，如果失败则普通打开
                        psi.FileName = "chrome.exe";
                        psi.Arguments = "--app=https://www.youtube.com";
                        break;
                    case "files":
                        psi.FileName = "explorer.exe";
                        break;
                }
                Process.Start(psi);
            }
            catch (Exception ex)
            {
                MessageBox.Show("启动失败，请确认已安装 Chrome。\n错误: " + ex.Message);
            }

            this.Hide();
        }

        // 处理换肤点击
        private void Theme_Click(object sender, RoutedEventArgs e)
        {
            PlaySound();
            Button btn = sender as Button;
            string colorHex = btn.Tag.ToString();

            try
            {
                var brush = (SolidColorBrush)new BrushConverter().ConvertFrom(colorHex);
                MainCard.Background = brush;

                // 简单的文字变色逻辑
                if (colorHex == "#202124") // 如果是深色模式
                {
                    SearchBox.Foreground = Brushes.White;
                }
                else
                {
                    SearchBox.Foreground = Brushes.Gray;
                }
            }
            catch { }
        }

        private void Close_Click(object sender, RoutedEventArgs e)
        {
            this.Hide();
        }
    }
}