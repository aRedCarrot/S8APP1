namespace CoupDeSonde.Models
{
    public class SurveyQuestion
    {
        public String Prompt { get; set; }
        public Int32 QuestionId { get; set; }
        public List<SurveyOption> Options { get; set; }

        public SurveyQuestion ()
        {
            Prompt = "";
            QuestionId = -1;
            Options = new List<SurveyOption>();
        }
    }

    public class SurveyOption
    {
        public String OptionTitle { get; set; }
        public String OptionValue { get; set; }

        public SurveyOption(string optionTitle, string optionValue)
        {
            OptionTitle = optionTitle;
            OptionValue = optionValue;
        }
    }
}
