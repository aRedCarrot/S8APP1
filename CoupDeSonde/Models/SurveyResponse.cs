namespace CoupDeSonde.Models
{
    public class SurveyResponse
    {
        public List<String> SurveyQuestions { get; set; }

        public SurveyResponse(List<String> surveyQuestions)
        {
            SurveyQuestions = surveyQuestions;
        }
    }
}
