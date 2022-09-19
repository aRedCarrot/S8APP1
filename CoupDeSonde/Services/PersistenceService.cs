using CoupDeSonde.Models;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace CoupDeSonde.Services
{
    public interface IPersistenceService
    {
        bool Save(SurveyResponse survey, string username);
    }
    public class PersistenceService : IPersistenceService
    {
        private AppSettings _appSettings;

        public PersistenceService(AppSettings appSettings)
        {
            _appSettings = appSettings;
        }

        private bool IsNewSurvey(SurveyResponse survey, string username)
        {
            var records = getRecords();
            if (!(records.ContainsKey(username)))
                return true; 

            // Check if an entry already exists
            foreach (SurveyResponse previousSurvey in records[username])
            {
                if (previousSurvey.SurveyId == survey.SurveyId) return false;
            }
            return true;
        }

        public bool Save(SurveyResponse survey, string username)
        {
            if (!IsNewSurvey(survey, username)) return false;
            var records = getRecords();

            if (!(records.ContainsKey(username)))
            {
                records.Add(username, new List<SurveyResponse>());
            }
            records[username].Add(survey);
            writeRecords(records);
            return true;
        }

        private Dictionary<string, List<SurveyResponse>> getRecords(){
            var path = Path.GetFullPath(_appSettings.persistencePath);
            if(File.Exists(path))
                return JsonSerializer.Deserialize<Dictionary<string, List<SurveyResponse>>>(File.ReadAllText(path));
            else
                return new Dictionary<string, List<SurveyResponse>>();
        } 

        private void writeRecords(Dictionary<string, List<SurveyResponse>> newRecords)
        {
            var path = Path.GetFullPath(_appSettings.persistencePath);
            var json = JsonSerializer.Serialize(newRecords);
            File.WriteAllText(path, json);
        }
            
    }
}
