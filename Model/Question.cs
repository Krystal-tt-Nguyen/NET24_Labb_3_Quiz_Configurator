
namespace Laboration_3.Model
{
    internal class Question
    {
        public string Query { get; set; }
        public string CorrectAnswer { get; set; }
        public string[] IncorrectAnswers { get; set; }

        public Question(string query, string correctAnswer, 
            string incorectAnswer1, string incorectAnswer2, string incorectAnswer3)
        {
            Query = query;
            CorrectAnswer = correctAnswer;
            IncorrectAnswers = new string[3] {incorectAnswer1, incorectAnswer2, incorectAnswer3 };
        }

    }
}
