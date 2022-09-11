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

        public SurveyService(IOptions<AppSettings> appSettings)
        {
            _appSettings = appSettings.Value;
        }

        public SurveyResponse GetSurvey(int SurveyId)
        {
            var path = Path.Combine(Directory.GetCurrentDirectory(), "sondage.txt");
            var text = System.IO.File.ReadAllText(path);
            var surveys = text.Split("\n\n");
            if(surveys.Length <= SurveyId)
                return new SurveyResponse(new List<String>());

            var questions = surveys[SurveyId].Split("\n");
            return new SurveyResponse(questions.ToList());
        }
    }
}
