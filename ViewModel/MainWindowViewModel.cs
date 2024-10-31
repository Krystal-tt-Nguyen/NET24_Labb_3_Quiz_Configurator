using Laboration_3.Command;
using Laboration_3.Dialogs;
using Laboration_3.Model;
using System.Collections.ObjectModel;

namespace Laboration_3.ViewModel
{
    internal class MainWindowViewModel : ViewModelBase
    {
        public ObservableCollection<QuestionPackViewModel> Packs { get; set; }
        public ConfigurationViewModel ConfigurationViewModel { get; }
        public PlayerViewModel PlayerViewModel { get; }
        public CreateNewPackDialog PackDialog { get; set; }


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

        private bool _removePackIsEnable;
        public bool RemovePackIsEnable
        {
            get => _removePackIsEnable;
            set
            {
                _removePackIsEnable = value;
                RaisePropertyChanged();
            }
        }


        public DelegateCommand CreateNewPackCommand { get; }
        public DelegateCommand ClosePackDialogCommand { get; }
        public DelegateCommand OpenPackDialogCommand { get; }
        public DelegateCommand RemovePackCommand { get; }
        public DelegateCommand SelectActivePackCommand { get; }


        public MainWindowViewModel()
        {
            RemovePackIsEnable = false;

            Packs = new ObservableCollection<QuestionPackViewModel>();
			ActivePack = new QuestionPackViewModel(new QuestionPack("Default Question Pack"));
            Packs.Add(ActivePack);
            ActivePack = Packs?.FirstOrDefault();

            ConfigurationViewModel = new ConfigurationViewModel(this);
			PlayerViewModel = new PlayerViewModel(this);

            OpenPackDialogCommand = new DelegateCommand(OpenNewPackDialog);
            CreateNewPackCommand = new DelegateCommand(CreateNewPack);
            ClosePackDialogCommand = new DelegateCommand(ClosePackDialog);
            RemovePackCommand = new DelegateCommand(RemovePack, IsRemovePackEnable);
            SelectActivePackCommand = new DelegateCommand(SelectActivePack);              
        }

        private void OpenNewPackDialog(object? obj)
        {
            NewPack = new QuestionPackViewModel(new QuestionPack());
            PackDialog = new CreateNewPackDialog();
            PackDialog.DataContext = this;
            PackDialog.Owner = System.Windows.Application.Current.MainWindow;
            PackDialog.ShowDialog(); 
        }
        private void CreateNewPack(object? obj)
        {
            if (NewPack != null)
            {
                Packs.Add(NewPack);
                ActivePack = NewPack;
                RemovePackCommand.RaiseCanExecuteChanged();
            }
            PackDialog.Close();
        }

        private void ClosePackDialog(object? obj) => PackDialog.Close();

        private void RemovePack(object? obj)
        {
            Packs.Remove(ActivePack);
            RemovePackCommand.RaiseCanExecuteChanged();
        }
        
        private bool IsRemovePackEnable(object? obj) => Packs != null && Packs.Count > 0 ? true : false;

        private void SelectActivePack(object? obj)
        {
            if (obj is QuestionPackViewModel selectedPack)
            {
                SelectedPack = selectedPack; 
                ActivePack = SelectedPack; 
            }
        }

        //public void ExitGame(object? obj) => Close(); !!!!!!!!

    }


}




//private object? _currentView;
//public object? CurrentView
//{
//    get => _currentView;
//    set
//    {
//        if (_currentView != value)
//        {
//            _currentView = value;
//            RaisePropertyChanged();
//        }
//    }
//}
//CurrentView = new ConfigurationView();
//public DelegateCommand SwitchToPlayerViewCommand { get; }
//public void SwitchToPlayerView(object? obj) => CurrentView = new PlayerView();
//SwitchToPlayerViewCommand = new DelegateCommand(SwitchToPlayerView);

