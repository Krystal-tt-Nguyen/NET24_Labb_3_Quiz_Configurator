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

        public DelegateCommand SwitchToPlayerViewCommand { get; }

        public MainWindowViewModel()
        {
			CurrentView = new ConfigurationView();

			ActivePack = new QuestionPackViewModel(new QuestionPack("Default Question Pack"));

            ConfigurationViewModel = new ConfigurationViewModel(this);
			PlayerViewModel = new PlayerViewModel(this);
            
			SwitchToPlayerViewCommand = new DelegateCommand(c => SwitchToPlayerView());
        }

		public void SwitchToPlayerView() => CurrentView = new PlayerView();

    }
}
