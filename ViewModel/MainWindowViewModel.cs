using Laboration_3.Command;
using Laboration_3.Model;
using Laboration_3.Views;
using System.Collections.ObjectModel;
using System.Windows.Input;

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
        public DelegateCommand SwitchToPlayerViewCommand { get; }


        public MainWindowViewModel()
        {
            RemovePackIsEnable = false;

			ActivePack = new QuestionPackViewModel(new QuestionPack("Default Question Pack"));

            CurrentView = new ConfigurationView();
            ConfigurationViewModel = new ConfigurationViewModel(this);
			PlayerViewModel = new PlayerViewModel(this);

            AddPackCommand = new DelegateCommand(AddPack);
            RemovePackCommand = new DelegateCommand(RemovePack, IsRemovePackEnable);
            SwitchToPlayerViewCommand = new DelegateCommand(SwitchToPlayerView);
        }

        private void AddPack(object? obj)
        {
            Packs.Add(new QuestionPackViewModel(new QuestionPack()));
            RemovePackCommand.RaiseCanExecuteChanged();
        }
        private void RemovePack(object? obj)
        {
            Packs.Remove(ActivePack);
            RemovePackCommand.RaiseCanExecuteChanged();
        }
        private bool IsRemovePackEnable(object? obj) => Packs.Count > 0 ? true : false;

        public void SwitchToPlayerView(object? obj) => CurrentView = new PlayerView();

    }


}


//public  DelegateCommand SwitchToResultViewCommand { get; }
//SwitchToResultViewCommand = new DelegateCommand(c => SwitchToResultView());
//public void SwitchToResultView() => CurrentView = new ResultView();
