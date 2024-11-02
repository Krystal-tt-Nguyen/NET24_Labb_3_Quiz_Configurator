using Laboration_3.Command;
using Laboration_3.Dialogs;
using Laboration_3.Model;
using System.Collections.ObjectModel;
using System.Windows;

namespace Laboration_3.ViewModel
{
    internal class MainWindowViewModel : ViewModelBase
    {
        public ObservableCollection<QuestionPackViewModel> Packs { get; set; }
        public ConfigurationViewModel ConfigurationViewModel { get; }
        public PlayerViewModel PlayerViewModel { get; }
        public CreateNewPackDialog PackDialog { get; set; }


        private bool _deletePackIsEnable;
        public bool DeletePackIsEnable
        {
            get => _deletePackIsEnable;
            set
            {
                _deletePackIsEnable = value;
                RaisePropertyChanged();
            }
        }


        private QuestionPackViewModel? _activePack;
		public QuestionPackViewModel? ActivePack
		{
			get => _activePack; 
			set 
			{ 
				_activePack = value;
				RaisePropertyChanged();
                ConfigurationViewModel?.RaisePropertyChanged();
            }
		}

        private QuestionPackViewModel? _newPack;
        public QuestionPackViewModel? NewPack
        {
            get => _newPack;
            set
            {
                _newPack = value;
                RaisePropertyChanged();
            }
        }

        private QuestionPackViewModel? _selectedPack;
        public QuestionPackViewModel? SelectedPack
        {
            get => _selectedPack;
            set
            {
                _selectedPack = value;
                RaisePropertyChanged();
            }
        }


        //public event EventHandler OpenDialogOnRequest; // FÖLJER INTE MVVM? - Bryt ut dialog till event

        public DelegateCommand ClosePackDialogCommand { get; }
        public DelegateCommand CreateNewPackCommand { get; }
        public DelegateCommand OpenPackDialogCommand { get; }
        public DelegateCommand DeletePackCommand { get; }
        public DelegateCommand SelectActivePackCommand { get; }


        public MainWindowViewModel()
        {
            DeletePackIsEnable = true;

            Packs = new ObservableCollection<QuestionPackViewModel>();
			ActivePack = new QuestionPackViewModel(new QuestionPack("Default Question Pack"));
            Packs.Add(ActivePack);
            ActivePack = Packs?.FirstOrDefault();

            ConfigurationViewModel = new ConfigurationViewModel(this);
			PlayerViewModel = new PlayerViewModel(this);

            ClosePackDialogCommand = new DelegateCommand(ClosePackDialog);
            CreateNewPackCommand = new DelegateCommand(CreateNewPack);
            OpenPackDialogCommand = new DelegateCommand(OpenNewPackDialog);
            DeletePackCommand = new DelegateCommand(DeletePack, IsDeletePackEnable);
            SelectActivePackCommand = new DelegateCommand(SelectActivePack);              
        }

        private void OpenNewPackDialog(object? obj)
        {
            NewPack = new QuestionPackViewModel(new QuestionPack());
            PackDialog = new CreateNewPackDialog();
            PackDialog.DataContext = this;
            PackDialog.Owner = System.Windows.Application.Current.MainWindow;
            PackDialog.ShowDialog();

            //OpenDialogOnRequest?.Invoke(this, EventArgs.Empty);
        }

        private void CreateNewPack(object? obj)
        {
            if (NewPack != null)
            {
                Packs.Add(NewPack);
                ActivePack = NewPack;
                DeletePackCommand.RaiseCanExecuteChanged();
            }

            PackDialog.Close();
        }

        private void ClosePackDialog(object? obj) => PackDialog.Close();

        private void DeletePack(object? obj)
        {
            MessageBoxResult result = MessageBox.Show($"Are you sure you want to delete \"{ActivePack.Name}\"?", 
                "Delete Question Pack?", MessageBoxButton.YesNo, MessageBoxImage.Warning);

            if (result == MessageBoxResult.Yes)
            {
                Packs.Remove(ActivePack);
                DeletePackCommand.RaiseCanExecuteChanged();
            }
            
            if (Packs.Count > 0)
            {
                ActivePack = Packs.FirstOrDefault();
            }
        }
        
        private bool IsDeletePackEnable(object? obj) => Packs != null && Packs.Count > 1 ? true : false;

        private void SelectActivePack(object? obj)
        {
            if (obj is QuestionPackViewModel selectedPack)
            {
                SelectedPack = selectedPack; 
                ActivePack = SelectedPack;
            }
        }


        // public void ExitGame(object? obj) => Close(); HUR AVSLUTA PROGRAMMET?!!!!!!!!
        // Application.Current.Shutdown();??

    }
}


//public event EventHandler OpenPackDialog;
//OpenPackDialog?.Invoke(this, EventArgs.Empty);
        
