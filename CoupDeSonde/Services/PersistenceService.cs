using CoupDeSonde.Models;
using Microsoft.Extensions.Options;
using System.Xml.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace CoupDeSonde.Services
{
    public interface IPersistenceService
    {
        void Save(SurveyResponse survey, string username);
    }
    public class PersistenceService : IPersistenceService
    {
        private AppSettings _appSettings;

        public PersistenceService(AppSettings appSettings)
        {
            _appSettings = appSettings;
        }

        public void Save(SurveyResponse survey, string username)
        {
            var path = Path.GetFullPath(_appSettings.persistencePath);
            var answer = JsonSerializer.Serialize(survey);
            
            File.AppendAllText(path, answer);
        }
    }
}
