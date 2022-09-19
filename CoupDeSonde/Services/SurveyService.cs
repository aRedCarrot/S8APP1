using CoupDeSonde.Models;
using Microsoft.Extensions.Options;

namespace CoupDeSonde.Services
{
    public interface ISurveyService
    {
        SurveyRequestResponse GetSurveys();
        bool SubmitSurvey(SurveyResponse response, string username);
    }

    public class SurveyService : ISurveyService
    {
        private readonly AppSettings _appSettings;
        private readonly IPersistenceService _persistenceService;
        public List<Survey> _surveys = new List<Survey>();

        public SurveyService(IOptions<AppSettings> appSettings)
        {
            _appSettings = appSettings.Value;
            _persistenceService = new PersistenceService(_appSettings);
            ParseSurveyFile();
        }

        public SurveyRequestResponse GetSurveys()
        {
            var response = new SurveyRequestResponse();
            if (_surveys.Count == 0)
            {
                response.Error = "There are currently no surveys available";
                return response;
            }
            response.Surveys = _surveys;
            return response;
        }

        public bool SubmitSurvey(SurveyResponse response, string username)
        {
            if (!IsValidResponse(response))
                return false;
            return _persistenceService.Save(response, username);
        }

        private bool IsValidResponse(SurveyResponse response)
        {
            //Check if survey id exist
            var survey = _surveys.SingleOrDefault(survey => survey.SurveyId == response.SurveyId,null);
            if (survey == null)
                return false;

            var surveyQuestions = survey.SurveyQuestions.Select(question => question.QuestionId);
            var responseQuestions = response.Responses.Select(response => response.QuestionId);
            //checked if all questions are answered
            if (surveyQuestions.Count() != responseQuestions.Count() || surveyQuestions.Except(responseQuestions).Any())
                return false;

            foreach (QuestionAnswer qa in response.Responses)
            {
                var question = survey.SurveyQuestions.Single(question => question.QuestionId == qa.QuestionId);

                //Check if option exist
                var answer = question.Options.SingleOrDefault(answer => answer.OptionTitle.ToUpper() == qa.Answer.ToUpper(),null);
                if (answer == null)
                    return false;
            }

            return true;
        }

        private void ParseSurveyFile()
        {
            var path = Path.Combine(Directory.GetCurrentDirectory(), "sondage.txt");
            if(!File.Exists(path))
                return;

            var text = File.ReadAllText(path);
            
            // Our teacher gave us a txt file with linux encoded end lines, while our dummy PC use windows end line
            var END_OF_LINE = "\n";
            if (text.Contains('\r'))
                END_OF_LINE = "\r\n";

            var surveys = text.Split(END_OF_LINE + END_OF_LINE);
            var surveyId = 0;
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
                newSurvey.SurveyId = surveyId;
                surveyId++;
                _surveys.Add(newSurvey);
            }
            Console.WriteLine("Parsed sondage.txt and found " + _surveys.Count + " survey(s)");
        }
    }
}
