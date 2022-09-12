using System.ComponentModel.DataAnnotations;

namespace CoupDeSonde.Models
{
    public class SurveyResponse
    {
        public String? Error { get; set; }

        // Used for sending a specific Survey
        public Survey? Survey { get; set; }

        // Used for sending a list of Surveys
        public List<Survey>? Surveys { get; set; }

        public SurveyResponse()
        {
            Error = null;
            Surveys = new List<Survey>();
        }
    }
}
