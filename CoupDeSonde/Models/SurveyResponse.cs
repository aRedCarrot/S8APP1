using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace CoupDeSonde.Models
{
    public class SurveyResponse
    {
        public Int32 SurveyId { get; set; }

        public List<QuestionAnswer> Responses { get; set; }

        public SurveyResponse(Int32 surveyId, List<QuestionAnswer> responses)
        {
            SurveyId = surveyId;
            Responses = responses;
        }
    }
}