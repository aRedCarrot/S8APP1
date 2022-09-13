
namespace CoupDeSonde.Models
{
    public class QuestionAnswer
    {
        public Int32 QuestionId { get; set; }
        public string Answer { get; set; }

        public QuestionAnswer(Int32 questionId, string answer)
        {
            QuestionId = questionId;
            Answer = answer;
        }
    }
}
