using System.ComponentModel.DataAnnotations;

namespace CoupDeSonde.Models
{
    public class SurveyResponse
    {
        public String? Error { get; set; }
        public List<SurveyQuestion> SurveyQuestions { get; set; }

        public SurveyResponse(List<SurveyQuestion> surveyQuestions, String? error = null)
        {
            SurveyQuestions = surveyQuestions;
            Error = error;
        }
    }
}
