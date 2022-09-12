namespace CoupDeSonde.Models
{
    public class QuestionAnswer
    {
        public Int32 QuestionId;
        public string Answer;

        public QuestionAnswer(Int32 questionId, string answer)
        {
            QuestionId = questionId;
            Answer = answer;
        }
    }
}
