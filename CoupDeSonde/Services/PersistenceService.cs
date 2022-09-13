using CoupDeSonde.Models;
using System.Collections.Generic;
using System.Text.Json;

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
            var newRecord = new SurveyRecordEntry(username, survey);
            var records = new List<SurveyRecordEntry>();
            try
            {
                records = JsonSerializer.Deserialize<List<SurveyRecordEntry>>(File.ReadAllText(path));
            }
            catch(Exception e)
            {
                Console.WriteLine("Failed to deserialize records file, recreating a new one");
            }
            finally
            {
                records?.Add(newRecord);
                var newRecords = JsonSerializer.Serialize(records);
                File.WriteAllText(path, newRecords);
            }
        }
    }
}
