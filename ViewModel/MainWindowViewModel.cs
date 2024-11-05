using Laboration_3.Command;
using Laboration_3.Dialogs;
using Laboration_3.JSON;
using Laboration_3.Model;
using System.Collections.ObjectModel;
using System.IO;
using System.Windows;
using System.Windows.Shapes;
using Path = System.IO.Path;

namespace Laboration_3.ViewModel
{
    internal class MainWindowViewModel : ViewModelBase
    {
        public ObservableCollection<QuestionPackViewModel> Packs { get; set; }
        public ConfigurationViewModel ConfigurationViewModel { get; }
        public PlayerViewModel PlayerViewModel { get; }
        public CreateNewPackDialog PackDialog { get; set; }

        private bool _canExit;
        public bool CanExit
        {
            get => _canExit;
            set
            {
                _canExit = value;
                RaisePropertyChanged();
                
            }
        }

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

        private bool _isFullscreen;
        public bool IsFullscreen
        {
            get => _isFullscreen;
            set 
            { 
                _isFullscreen = value;
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


        public event EventHandler<bool> ToggleFullScreenRequested;
        public event EventHandler <bool> ExitGameRequested;

        public DelegateCommand ClosePackDialogCommand { get; }
        public DelegateCommand CreateNewPackCommand { get; }
        public DelegateCommand OpenPackDialogCommand { get; }
        public DelegateCommand DeletePackCommand { get; }
        public DelegateCommand SelectActivePackCommand { get; }
        public DelegateCommand ToggleWindowFullScreenCommand { get; }
        public DelegateCommand ExitGameCommand { get; }


        //private JsonFileHandler JsonFileHandler;
        private string filePath;


        public MainWindowViewModel()
        {
            DeletePackIsEnable = true;
            IsFullscreen = false;
            CanExit = false;

            Packs = new ObservableCollection<QuestionPackViewModel>();
            ActivePack = new QuestionPackViewModel(new QuestionPack("Default Question Pack"));
            Packs.Add(ActivePack);
            ActivePack = Packs?.FirstOrDefault();

            //JsonFileHandler = new JsonFileHandler();
            filePath = JsonFileHandler.GetFilePath();

            if (Path.Exists(filePath))
            {
                JsonFileHandler.WriteToJson(filePath, Packs);
            }

            ConfigurationViewModel = new ConfigurationViewModel(this);
			PlayerViewModel = new PlayerViewModel(this);

            ClosePackDialogCommand = new DelegateCommand(ClosePackDialog);
            CreateNewPackCommand = new DelegateCommand(CreateNewPack);
            OpenPackDialogCommand = new DelegateCommand(OpenNewPackDialog);
            DeletePackCommand = new DelegateCommand(DeletePack, IsDeletePackEnable);
            SelectActivePackCommand = new DelegateCommand(SelectActivePack);
            ToggleWindowFullScreenCommand = new DelegateCommand(ToggleWindowFullScreen);
            ExitGameCommand = new DelegateCommand(ExitGame);
        }

        private void OpenNewPackDialog(object? obj) // Bryta ut - MVVM?
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

                ConfigurationViewModel.DeleteQuestionCommand.RaiseCanExecuteChanged();
                DeletePackCommand.RaiseCanExecuteChanged();
                JsonFileHandler?.WriteToJson(filePath, Packs);
            }
            PackDialog.Close();
        }

        private void ClosePackDialog(object? obj) => PackDialog.Close(); // Bryta ut - MVVM?

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

            JsonFileHandler?.WriteToJson(filePath, Packs);
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

        private void ToggleWindowFullScreen(object? obj)
        { 
            IsFullscreen = !IsFullscreen;
            ToggleFullScreenRequested?.Invoke(this, _isFullscreen);
        } //innan public
       
        private async void ExitGame(object? obj)
        {
            await JsonFileHandler.WriteToJson(filePath, Packs);

            CanExit = true;
            ExitGameRequested?.Invoke(this, CanExit);
        }

    }
}


//OpenDialogOnRequest?.Invoke(this, EventArgs.Empty);