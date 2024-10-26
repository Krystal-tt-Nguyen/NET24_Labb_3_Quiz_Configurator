using Laboration_3.Model;
using Laboration_3.ViewModel;
using System.Windows;
using System.Windows.Automation.Peers;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Windows.Input;

namespace Laboration_3
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            DataContext = new MainWindowViewModel();
        }

        private void CustomTitleBar_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ButtonState == MouseButtonState.Pressed && e.ChangedButton == MouseButton.Left)
            {
                DragMove();
            }
        }

        private void btnMinimize_Click(object sender, RoutedEventArgs e)
            => WindowState = (WindowState == WindowState.Normal) ? WindowState.Minimized : WindowState.Normal;

        private void btnFullscreen_Click(object sender, RoutedEventArgs e) 
            => WindowState = (WindowState == WindowState.Normal) ? WindowState.Maximized : WindowState.Normal;

        private void btnClose_Click(object sender, RoutedEventArgs e) => Close();

    }

}