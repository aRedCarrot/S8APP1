using CoupDeSonde.Models;
using Microsoft.Extensions.Options;

namespace CoupDeSonde.Services
{
    public interface ISurveyService
    {
        SurveyResponse GetSurvey(int SurveyId);
    }

    public class SurveyService : ISurveyService
    {
        private readonly AppSettings _appSettings;
        private List<List<SurveyQuestion>> _surveys = new List<List<SurveyQuestion>>();

        public SurveyService(IOptions<AppSettings> appSettings)
        {
            _appSettings = appSettings.Value;
            ParseSurveyFile();
        }

        public SurveyResponse GetSurvey(int SurveyId)
        {
            if(_surveys.Count == 0)
                return new SurveyResponse(new List<SurveyQuestion>(), "There are currently no surveys available");
            if (SurveyId > _surveys.Count -1)
                return new SurveyResponse(new List<SurveyQuestion>(), "Invalid SurveyId, Available survey Ids are [0," + (_surveys.Count-1) + "]");
            return new SurveyResponse(_surveys[SurveyId]);
        }

        private void ParseSurveyFile()
        {
            var path = Path.Combine(Directory.GetCurrentDirectory(), "sondage.txt");
            var text = File.ReadAllText(path);
            var surveys = text.Split("\r\n\r\n");
            foreach (String survey in surveys)
            {
                var newSurvey = new List<SurveyQuestion>();
                var questions = survey.Split("\r\n");
                var questionId = 1;
                foreach(String question in questions)
                {
                    String prompt = question.Split("?")[0].Split(".")[1].Trim() + "?";
                    String options = question.Split("?")[1];
                    var newQuestion = new SurveyQuestion();
                    newQuestion.Prompt = prompt;
                    newQuestion.QuestionId = questionId;
                    int indexOfSemiColon = options.IndexOf(":");
                    while(indexOfSemiColon != -1)
                    {
                        string optionTitle = options.Substring(indexOfSemiColon - 1, 1);
                        int endIndex = options.IndexOf(",",indexOfSemiColon) - 1;
                        if (endIndex < 0)
                            endIndex = options.Length -1;
                        string optionValue = options.Substring(indexOfSemiColon+1, endIndex - indexOfSemiColon);
                        indexOfSemiColon = options.IndexOf(":", endIndex);

                        newQuestion.Options.Add(new SurveyOption(optionTitle, optionValue));
                    }
                    newSurvey.Add(newQuestion);
                    questionId++;
                }
                _surveys.Add(newSurvey);
            }
            Console.WriteLine("Parsed sondage.txt and found " + _surveys.Count + " survey(s)");
        }
    }
}
