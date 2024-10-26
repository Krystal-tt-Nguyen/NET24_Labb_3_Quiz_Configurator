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

		private object _currentView;
		public object CurrentView
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

        public ICommand SwitchToPlayerViewCommand { get; }
        public ICommand SwitchToResultViewCommand { get; }


        public MainWindowViewModel()
        {
			// Ange default vy vid programstart
			CurrentView = new ConfigurationView();

            ConfigurationViewModel = new ConfigurationViewModel(this);
			PlayerViewModel = new PlayerViewModel(this);

            SwitchToPlayerViewCommand = new DelegateCommand(c => SwitchToPlayerView());
            //SwitchToResultViewCommand = new DelegateCommand(c => SwitchToResultView());
        }

		public void SwitchToPlayerView() => CurrentView = new PlayerView();
		//public void SwitchToResultView() => CurrentView = new ResultView();

    }
}
