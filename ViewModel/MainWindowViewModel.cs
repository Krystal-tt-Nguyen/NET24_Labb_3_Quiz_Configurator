using Laboration_3.Command;
using Laboration_3.Dialogs;
using Laboration_3.Model;
using Laboration_3.Views;
using System.Collections.ObjectModel;

namespace Laboration_3.ViewModel
{
    internal class MainWindowViewModel : ViewModelBase
    {
        public ObservableCollection<QuestionPackViewModel> Packs { get; set; }
        public ConfigurationViewModel ConfigurationViewModel { get; }
        public PlayerViewModel PlayerViewModel { get; }


        private QuestionPackViewModel? _activePack;
		public QuestionPackViewModel? ActivePack
		{
			get => _activePack; 
			set 
			{ 
				_activePack = value;
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

        private object? _currentView;
		public object? CurrentView
		{
			get => _currentView; 
			set 
			{ 
				if (_currentView != value)
				{
					_currentView = value;
					RaisePropertyChanged();
				}
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


        public DelegateCommand AddPackCommand { get; }
        public DelegateCommand RemovePackCommand { get; }
        public DelegateCommand SelectActivePackCommand { get; }
        public DelegateCommand SwitchToPlayerViewCommand { get; }


        public MainWindowViewModel()
        {
            RemovePackIsEnable = false;

            Packs = new ObservableCollection<QuestionPackViewModel>();
			ActivePack = new QuestionPackViewModel(new QuestionPack("Default Question Pack"));
            Packs.Add(ActivePack);
            ActivePack = Packs?.FirstOrDefault();

            CurrentView = new ConfigurationView();
            ConfigurationViewModel = new ConfigurationViewModel(this);
			PlayerViewModel = new PlayerViewModel(this);

            AddPackCommand = new DelegateCommand(AddPack);
            RemovePackCommand = new DelegateCommand(RemovePack, IsRemovePackEnable);
            SelectActivePackCommand = new DelegateCommand(SelectActivePack);
            SwitchToPlayerViewCommand = new DelegateCommand(SwitchToPlayerView);
        }

        private void AddPack(object? obj)
        {
            // Dela upp metode, finns två knappar -> Create = Add, Cancel = Close + enbart öppna dialogfönstret
            var createNewPackDialog = new CreateNewPackDialog();
            createNewPackDialog.DataContext = this;
            createNewPackDialog.Owner = System.Windows.Application.Current.MainWindow;
            createNewPackDialog.ShowDialog(); 
            
            Packs.Add(new QuestionPackViewModel(new QuestionPack()));
            RemovePackCommand.RaiseCanExecuteChanged();
        }

        private void RemovePack(object? obj)
        {
            // Funkar EJ
            //Packs.Remove(ActivePack);
            //RemovePackCommand.RaiseCanExecuteChanged();
        }
        
        private bool IsRemovePackEnable(object? obj) => Packs != null && Packs.Count > 0 ? true : false;

        private void SelectActivePack(object? obj) => ActivePack = SelectedPack;

        public void SwitchToPlayerView(object? obj) => CurrentView = new PlayerView();

        //public void ExitGame(object? obj) => Close(); !!!!!!!!

    }


}

//private QuestionPackViewModel? _newPack;
//public QuestionPackViewModel? NewPack
//{
//    get => _newPack;
//    set
//    {
//        _newPack = value;
//        RaisePropertyChanged();
//    }
//}

//public  DelegateCommand SwitchToResultViewCommand { get; }
//SwitchToResultViewCommand = new DelegateCommand(c => SwitchToResultView());
//public void SwitchToResultView() => CurrentView = new ResultView();
