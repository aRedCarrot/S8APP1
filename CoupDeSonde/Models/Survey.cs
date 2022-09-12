namespace CoupDeSonde.Models
{
    public class Survey
    {
        public string Title { get; set; }
        public List<SurveyQuestion> SurveyQuestions { get; set; }

        public Survey(string? title)
        {
            Title = title ?? "";
            SurveyQuestions = new List<SurveyQuestion>();
        }
    }
}
