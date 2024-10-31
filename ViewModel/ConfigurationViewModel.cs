using Laboration_3.Command;
using Laboration_3.Dialogs;
using Laboration_3.Model;
using System.Windows;

namespace Laboration_3.ViewModel
{
    internal class ConfigurationViewModel : ViewModelBase
    {
        private readonly MainWindowViewModel? mainWindowViewModel;
        public QuestionPackViewModel? ActivePack { get => mainWindowViewModel.ActivePack; }


        private bool _removeQuestionIsEnable;
        public bool RemoveQuestionIsEnable
        {
            get => _removeQuestionIsEnable;
            set 
            {
                _removeQuestionIsEnable = value; 
                RaisePropertyChanged();
            }
        }

        private Question? _selectedQuestion;
        public Question? SelectedQuestion
        {
            get => _selectedQuestion;
            set
            {
                _selectedQuestion = value;
                RaisePropertyChanged();
                ChangeTextVisibility();
            }
        }

        private Visibility _textVisibility;
        public Visibility TextVisibility
        {
            get => _textVisibility;
            set 
            { 
                _textVisibility = value;
                RaisePropertyChanged();
            }
        }


        public DelegateCommand AddQuestionCommand { get; }
        public DelegateCommand RemoveQuestionCommand { get; }
        public DelegateCommand EditPackOptionsCommand { get; }


        public ConfigurationViewModel(MainWindowViewModel? mainWindowViewModel)
        {
            this.mainWindowViewModel = mainWindowViewModel;
            
            RemoveQuestionIsEnable = false;
            SelectedQuestion = ActivePack?.Questions.FirstOrDefault();
            TextVisibility = ActivePack?.Questions.Count > 0 ? Visibility.Visible : Visibility.Hidden;
            
            AddQuestionCommand = new DelegateCommand(AddQuestion); 
            RemoveQuestionCommand = new DelegateCommand(RemoveQuestion, IsRemoveQuestionEnable);
            EditPackOptionsCommand = new DelegateCommand(EditPackOptions);
        }

        private void AddQuestion(object? obj)
        { 
            ActivePack?.Questions.Add(new Question(string.Empty, string.Empty, string.Empty, string.Empty, string.Empty));
            SelectedQuestion = (ActivePack?.Questions.Count > 0) ? ActivePack?.Questions.Last() : ActivePack?.Questions.FirstOrDefault();
            RemoveQuestionCommand.RaiseCanExecuteChanged();
            ChangeTextVisibility();

        }
        private void RemoveQuestion(object? obj)
        { 
            ActivePack?.Questions.Remove(SelectedQuestion);
            RemoveQuestionCommand.RaiseCanExecuteChanged();
            ChangeTextVisibility();
        }

        private bool IsRemoveQuestionEnable(object? obj) 
            => RemoveQuestionIsEnable = ActivePack != null && ActivePack?.Questions.Count > 0 ? true : false;

        private void EditPackOptions(object? obj)
        {
            var packOptionsDialog = new PackOptionsDialog();
            packOptionsDialog.DataContext = this;
            packOptionsDialog.Owner = Application.Current.MainWindow;
            packOptionsDialog.ShowDialog();
        }

        private void ChangeTextVisibility() 
            => TextVisibility = ActivePack?.Questions.Count > 0 && SelectedQuestion != null ? Visibility.Visible : Visibility.Hidden;
    }
}


