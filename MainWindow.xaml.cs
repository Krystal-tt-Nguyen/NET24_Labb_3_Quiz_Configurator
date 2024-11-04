using Laboration_3.ViewModel;
using System.Windows;

namespace Laboration_3
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            var mainWindowViewModel = new MainWindowViewModel();
            DataContext = mainWindowViewModel;

            mainWindowViewModel.ToggleFullScreenRequested += OnToggleFullScreenRequested;
            mainWindowViewModel.ExitGameRequested += OnExitRequested;
        }

        public void OnToggleFullScreenRequested (object? sender, bool isFullscreen)
        {
            if (isFullscreen)
            {
                this.WindowState = WindowState.Maximized;
                this.WindowStyle = WindowStyle.None;
            }
            else
            {
                this.WindowState = WindowState.Normal;
                this.WindowStyle = WindowStyle.SingleBorderWindow;
            }
        }

        public void OnExitRequested(object? obj, bool canExit)
        {
            if (canExit)
            {
                var result = System.Windows.MessageBox.Show("Are you sure you want to exit the game?", "Exit Game?",
                    MessageBoxButton.YesNo, MessageBoxImage.Warning);

                if (result == MessageBoxResult.Yes)
                {
                    Application.Current.MainWindow.Close();
                }
            }
        }

    }

}


// MVVM - bryta ut dialogrutor till events?
//private readonly MainWindowViewModel? mainWindowViewModel;
//private void OnOpenOnRequestDialog(object sender, EventArgs e)
//{
//    var PackDialog = new CreateNewPackDialog() {DataContext = mainWindowViewModel.NewPack};
//    PackDialog.ShowDialog();
//}
//mainWindowViewModel.OpenDialogOnRequest += OnOpenOnRequestDialog;