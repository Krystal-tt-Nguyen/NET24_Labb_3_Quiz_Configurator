using Laboration_3.Command;
using Laboration_3.Dialogs;
using Laboration_3.Model;
using System.Collections.ObjectModel;
using System.IO;
using System.Text.Json;
using System.Windows;

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


        public MainWindowViewModel()
        {
            DeletePackIsEnable = true;
            IsFullscreen = false;
            CanExit = false;

            //string path = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);

       //     if (Path.Exists(path))
       //     {
       //         WriteToJson();
       //     }
       //     else
       //     {
       //         Packs = new ObservableCollection<QuestionPackViewModel>();
			    //ActivePack = new QuestionPackViewModel(new QuestionPack("Default Question Pack"));
            //         Packs.Add(ActivePack);
            //         ActivePack = Packs?.FirstOrDefault();
            //     }

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

            //OpenDialogOnRequest?.Invoke(this, EventArgs.Empty);
        }

        private void CreateNewPack(object? obj)
        {
            if (NewPack != null)
            {
                Packs.Add(NewPack);
                ActivePack = NewPack;

                ConfigurationViewModel.DeleteQuestionCommand.RaiseCanExecuteChanged();
                DeletePackCommand.RaiseCanExecuteChanged();
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

        public void ToggleWindowFullScreen(object? obj)
        { 
            IsFullscreen = !IsFullscreen;
            ToggleFullScreenRequested?.Invoke(this, _isFullscreen);
        }
       
        public async void ExitGame(object? obj)
        {
            //await WriteToJson();
            
            CanExit = true;
            ExitGameRequested?.Invoke(this, CanExit);
        }


        //public async Task WriteToJson()
        //{
        //    var options = new JsonSerializerOptions()
        //    {
        //        IncludeFields = true,
        //        IgnoreReadOnlyProperties = false
        //    };

        //    string json = JsonSerializer.Serialize(Packs, options);
        //    string path = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);

        //    path = Path.Combine(path, "Laboration_3");
        //    Directory.CreateDirectory(path);
        //    path = Path.Combine(path, "Laboration_3.json");

        //    File.WriteAllText(path, json);
        //}

        //public async Task ReadFromJson()
        //{

        //}

    }
}

