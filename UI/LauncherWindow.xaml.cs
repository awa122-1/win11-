using System.Windows;
using ChromeOSLauncher.Core;

namespace ChromeOSLauncher.UI
{
    public partial class LauncherWindow : Window
    {
        public LauncherWindow()
        {
            InitializeComponent();
        }

        // 窗口加载完成
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            AutoStartCheck.IsChecked = AutoStartManager.IsEnabled();
        }

        private void AutoStart_Checked(object sender, RoutedEventArgs e)
        {
            AutoStartManager.Enable();
        }

        private void AutoStart_Unchecked(object sender, RoutedEventArgs e)
        {
            AutoStartManager.Disable();
        }
    }
}
