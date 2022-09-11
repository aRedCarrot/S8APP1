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
            var path = Path.Combine(Directory.GetCurrentDirectory(), "\\sondage.txt");
            Console.WriteLine(path);
            //var text = System.IO.File.ReadAllText(path);
            //Console.WriteLine(text);
            return new SurveyResponse(new List<String>());
        }
    }
}
