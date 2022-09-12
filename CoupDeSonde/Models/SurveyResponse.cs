using System.ComponentModel.DataAnnotations;

namespace CoupDeSonde.Models
{
    public class SurveyResponse
    {
        public Int32 SurveyId { get; set; }

        public List<QuestionAnswer> Responses { get; set; }

        public SurveyRequestResponse(Int32 surveyId, List<QuestionAnswer> responses)
        {
            SurveyId = surveyId;
            Responses = responses;
        }
    }
}