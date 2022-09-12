using System.ComponentModel.DataAnnotations;

namespace CoupDeSonde.Models
{
    public class SurveyRequestResponse
    {
        public String? Error { get; set; }

        // Used for sending a specific Survey
        public Survey? Survey { get; set; }

        // Used for sending a list of Surveys
        public List<Survey>? Surveys { get; set; }

        public SurveyRequestResponse()
        {
            Error = null;
            Surveys = new List<Survey>();
        }
    }
}
