using Laboration_3.Dialogs;
using Laboration_3.Model;
using Laboration_3.ViewModel;
using System.Windows.Controls;

namespace Laboration_3.Views
{
    public partial class MenuView : UserControl
    {
        // MVVM - bryta ut dialogrutor till events?

        //private readonly MainWindowViewModel? mainWindowViewModel;
        public MenuView()
        {
            InitializeComponent();
            //mainWindowViewModel.OpenDialogOnRequest += OnOpenOnRequestDialog;
        }

        //private void OnOpenOnRequestDialog(object sender, EventArgs e)
        //{
        //    var PackDialog = new CreateNewPackDialog() {DataContext = mainWindowViewModel.NewPack};
        //    PackDialog.ShowDialog();
        //}
    }
}
