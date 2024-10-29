using Laboration_3.Command;
using Laboration_3.Model;
using Laboration_3.Dialogs;
using Laboration_3.ViewModel;
using System.Windows;
using System.Windows.Input;

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
            
            AddQuestionCommand = new DelegateCommand(AddQuestion); 
            RemoveQuestionCommand = new DelegateCommand(RemoveQuestion, IsButtonEnable);
            EditPackOptionsCommand = new DelegateCommand(EditPackOptions);
        }

        public void AddQuestion(object? obj)
        { 
            ActivePack?.Questions.Add(new Question(string.Empty, string.Empty, string.Empty, string.Empty, string.Empty));
            RemoveQuestionCommand.RaiseCanExecuteChanged();
        }
        public void RemoveQuestion(object? obj)
        { 
            ActivePack?.Questions.Remove(SelectedQuestion);
            RemoveQuestionCommand.RaiseCanExecuteChanged();
        }
        public bool IsButtonEnable(object? obj) => RemoveQuestionIsEnable = (ActivePack?.Questions.Count > 0) ? true : false;
        public void EditPackOptions(object? obj)
        {
            var packOptionsDialog = new PackOptionsDialog();
            packOptionsDialog.Owner = Application.Current.MainWindow;
            packOptionsDialog.Show();
        }

    }
}

//Focus
//SelectedQuestion = (ActivePack?.Questions.Count > 0) ? ActivePack.Questions.First() : null;

