using Laboration_3.Command;
using Laboration_3.Model;
using System.Windows;
using System.Windows.Threading;

namespace Laboration_3.ViewModel
{
    internal class PlayerViewModel : ViewModelBase
    {
        private readonly MainWindowViewModel? mainWindowViewModel;
        public QuestionPackViewModel? ActivePack { get => mainWindowViewModel.ActivePack; }


        private DispatcherTimer timer;

        private int _timeLimit;
        public int TimeLimit
        {
            get => _timeLimit;
            private set
            {
                _timeLimit = value;
                RaisePropertyChanged();
            }
        }


        private int correctAnswerIndex;
        private int currentQuestionIndex;
        private int playerAnswerIndex;
        private int amountcorrectAnswers;
        private Random rnd = new Random();

        private string _correctAnswer;
        public string CorrectAnswer
        {
            get => _correctAnswer;
            set 
            { 
                _correctAnswer = value;
                RaisePropertyChanged();
            }
        }

        private string _currentQuestion;
        public string CurrentQuestion
        {
            get => _currentQuestion;
            set
            {
                _currentQuestion = value;
                RaisePropertyChanged();
            }
        }

        private string _questionStatus;
        public string QuestionStatus
        {
            get => _questionStatus;
            set
            {
                _questionStatus = value;
                RaisePropertyChanged();
            }
        }

        private string _results;
        public string Results
        {
            get => _results;
            set 
            { 
                _results = value;
                RaisePropertyChanged();
            }
        }


        private List<string> _shuffledAnswers;
        public List<string> ShuffledAnswers
        {
            get =>_shuffledAnswers; 
            set 
            { 
                _shuffledAnswers = value;
                RaisePropertyChanged();
            }
        }

        private List<Question> _questions;
        public List<Question> Questions
        {
            get => _questions;
            set
            {
                _questions = value;
                RaisePropertyChanged();
            }
        }


        private bool _isPlayerModeVisible;
        public bool IsPlayerModeVisible
        {
            get => _isPlayerModeVisible;
            set 
            {
                _isPlayerModeVisible = value;
                RaisePropertyChanged();
            }
        }

        private bool _isResultModeVisible;
        public bool IsResultModeVisible
        {
            get => _isResultModeVisible;
            set
            {
                _isResultModeVisible = value;
                RaisePropertyChanged();
            }
        }


        private Visibility[] _checkmarkVisibilities;
        public Visibility[] CheckmarkVisibilities
        {
            get => _checkmarkVisibilities;
            set
            {
                _checkmarkVisibilities = value;
                RaisePropertyChanged();
            }
        }

        private Visibility[] _crossVisibilities;
        public Visibility[] CrossVisibilities
        {
            get => _crossVisibilities;
            set
            {
                _crossVisibilities = value;
                RaisePropertyChanged();
            }
        }


        public DelegateCommand SwitchToPlayModeCommand { get; }
        public DelegateCommand CheckPlayerAnswerCommand { get; }

        public PlayerViewModel(MainWindowViewModel? mainWindowViewModel)
        {
            this.mainWindowViewModel = mainWindowViewModel;
            
            SwitchToPlayModeCommand = new DelegateCommand(StartPlayMode, IsPlayModeEnable);
            CheckPlayerAnswerCommand = new DelegateCommand(OnSelectedAnswer);

            CheckmarkVisibilities = new Visibility[4] {Visibility.Hidden, Visibility.Hidden, Visibility.Hidden, Visibility.Hidden };
            CrossVisibilities = new Visibility[4] {Visibility.Hidden, Visibility.Hidden, Visibility.Hidden, Visibility.Hidden };
        }

        private void StartPlayMode(object? obj)
        {
            IsPlayerModeVisible = true;
            IsResultModeVisible = false;
            mainWindowViewModel.ConfigurationViewModel.IsConfigurationModeVisible = false;

            if (timer == null)
            {
                timer = new DispatcherTimer();
                timer.Interval = TimeSpan.FromSeconds(1);
                timer.Tick += OnTimerTick;
            }

            Questions = ActivePack.Questions.ToList();
            currentQuestionIndex = 0;
            amountcorrectAnswers = 0;

            LoadNextQuestion();
        }

        private bool IsPlayModeEnable(object? obj) => ActivePack.Questions.Count > 0 ? true : false;

        private void OnTimerTick(object? sender, EventArgs e)
        {
            if (TimeLimit > 0)
            {
                TimeLimit--;
            }
            else
            {
                timer.Stop();
                AwaitDisplayCorrectAnswer();
            }
        }

        private void LoadNextQuestion()
        {
            TimeLimit = ActivePack.TimeLimitInSeconds;
            timer.Start();

            ResetChecksAndCrossVisibility();

            if (currentQuestionIndex < Questions.Count)
            {
                QuestionStatus = $"Question {currentQuestionIndex + 1} of {Questions.Count}";
                PrepareQuestion();
            }
            else
            {
                SwitchToResultView();
            }
        }

        private void PrepareQuestion()
        {
            CurrentQuestion = ActivePack.Questions[currentQuestionIndex].Query;
            CorrectAnswer = ActivePack.Questions[currentQuestionIndex].CorrectAnswer;

            List<string> Answers = new List<string>
            {
                ActivePack.Questions[currentQuestionIndex].CorrectAnswer,
                ActivePack.Questions[currentQuestionIndex].IncorrectAnswers[0],
                ActivePack.Questions[currentQuestionIndex].IncorrectAnswers[1],
                ActivePack.Questions[currentQuestionIndex].IncorrectAnswers[2]
            };
  
            ShuffledAnswers = Answers.OrderBy(a => rnd.Next()).ToList();
            currentQuestionIndex++;
        }

        private async void OnSelectedAnswer(object? obj)
        {
            correctAnswerIndex = ShuffledAnswers.IndexOf(CorrectAnswer);

            if (obj == null) /// FIXA När spelaren ej trycker på något
            {
                await DisplayCorrectAnswer();
                return;
            }
  
            var playerAnswer = obj as string;
            playerAnswerIndex = ShuffledAnswers.IndexOf(playerAnswer);

            await DisplayCorrectAnswer();
        }

        private async Task DisplayCorrectAnswer()
        {
            timer.Stop();

            if (playerAnswerIndex == correctAnswerIndex)
            {
                amountcorrectAnswers++;
                CheckmarkVisibilities[playerAnswerIndex] = Visibility.Visible;
            }
            else
            {
                CrossVisibilities[playerAnswerIndex] = Visibility.Visible;
            }

            CheckmarkVisibilities[correctAnswerIndex] = Visibility.Visible;

            RaisePropertyChanged("CheckmarkVisibilities");
            RaisePropertyChanged("CrossVisibilities");

            await Task.Delay(2000);

            LoadNextQuestion(); 
        }

        private async void AwaitDisplayCorrectAnswer() => await DisplayCorrectAnswer();

        private void ResetChecksAndCrossVisibility()
        {
            for (int i = 0; i < CheckmarkVisibilities.Length; i++)
            {
                CheckmarkVisibilities[i] = Visibility.Hidden;
                CrossVisibilities[i] = Visibility.Hidden;
            }

            RaisePropertyChanged("CheckmarkVisibilities");
            RaisePropertyChanged("CrossVisibilities");
        }

        public void SwitchToResultView()
        {
            timer.Stop();
            timer = null;

            Results = $"You got {amountcorrectAnswers} out of {Questions.Count} answers correct";

            IsResultModeVisible = true;
            IsPlayerModeVisible = false;
        }
       
    }
}
