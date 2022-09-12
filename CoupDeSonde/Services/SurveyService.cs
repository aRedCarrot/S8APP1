using CoupDeSonde.Models;
using Microsoft.Extensions.Options;

namespace CoupDeSonde.Services
{
    public interface ISurveyService
    {
        SurveyResponse GetSurvey(int SurveyId);
        SurveyResponse GetSurveys();
    }

    public class SurveyService : ISurveyService
    {
        private readonly AppSettings _appSettings;
        private List<Survey> _surveys = new List<Survey>();

        public SurveyService(IOptions<AppSettings> appSettings)
        {
            _appSettings = appSettings.Value;
            ParseSurveyFile();
        }

        public SurveyResponse GetSurvey(int SurveyId)
        {
            var response = new SurveyResponse();
            if (SurveyId > _surveys.Count  || SurveyId < 1)
            {
                response.Error = "Invalid SurveyId, Available survey Ids are [1," + (_surveys.Count) + "]";
                return response;
            }
            response.Survey = _surveys[SurveyId - 1];
            return response;
        }

        public SurveyResponse GetSurveys()
        {
            var response = new SurveyResponse();
            if (_surveys.Count == 0)
            {
                response.Error = "There are currently no surveys available";
                return response;
            }
            response.Surveys = _surveys;
            return response;
        }

        private void ParseSurveyFile()
        {
            var path = Path.Combine(Directory.GetCurrentDirectory(), "sondage.txt");
            var text = File.ReadAllText(path);
            
            // Our teacher gave us a txt file with linux encoded end lines, while our dummy PC use windows end line
            // Must adapt which one it is
            var END_OF_LINE = "\n";
            if (text.Contains('\r'))
                END_OF_LINE = "\r\n";

            var surveys = text.Split(END_OF_LINE + END_OF_LINE);
            foreach (String survey in surveys)
            {
                var questions = survey.Split(END_OF_LINE).ToList();
                var newSurvey = new Survey(questions[0].Remove(questions[0].Length -1));
                // The first "question" is the title of the survey so we pop it
                questions.RemoveAt(0);
                var questionId = 1;
                foreach(String question in questions)
                {
                    if (question.Length == 0) continue;
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
                    newSurvey.SurveyQuestions.Add(newQuestion);
                    questionId++;
                }
                _surveys.Add(newSurvey);
            }
            Console.WriteLine("Parsed sondage.txt and found " + _surveys.Count + " survey(s)");
        }
    }
}
