namespace CoupDeSonde.Models
{
    public class SurveyRecordEntry
    {
        public String Username { get; set; }
        public SurveyResponse SurveyResponse { get; set; }

        public SurveyRecordEntry(String username, SurveyResponse surveyResponse)
        {
            Username = username;
            SurveyResponse = surveyResponse;
        }
    }
}
