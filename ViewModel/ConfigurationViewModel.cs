using Laboration_3.Command;
using Laboration_3.Model;
using Laboration_3.ViewModel;
using System.Windows;
using System.Windows.Input;

namespace Laboration_3.ViewModel
{
    internal class ConfigurationViewModel : ViewModelBase
    {
        private readonly MainWindowViewModel? mainWindowViewModel;
        public QuestionPackViewModel? ActivePack { get => mainWindowViewModel.ActivePack; }


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

        private bool _buttonisEnable;
        public bool buttonIsEnable
        {
            get => _buttonisEnable;
            set 
            {
                _buttonisEnable = value; 
                RaisePropertyChanged();
            }
        }


        public DelegateCommand AddQuestionCommand { get; }
        public DelegateCommand RemoveQuestionCommand { get; }


        public ConfigurationViewModel(MainWindowViewModel? mainWindowViewModel)
        {
            this.mainWindowViewModel = mainWindowViewModel;
            buttonIsEnable = false;
            SelectedQuestion = ActivePack?.Questions.FirstOrDefault();
            
            AddQuestionCommand = new DelegateCommand(AddQuestion); 
            RemoveQuestionCommand = new DelegateCommand(RemoveQuestion, IsButtonEnable);
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
        public bool IsButtonEnable(object? obj) => buttonIsEnable = (ActivePack?.Questions.Count > 0) ? true : false;
        public void EditPackOptions()
        {
            
        }
        
      
    }
}



