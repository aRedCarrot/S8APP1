using CoupDeSonde;
using CoupDeSonde.Controllers;
using CoupDeSonde.Models;
using CoupDeSonde.Services;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;

namespace UnitTests.Services
{
    public class SurveyServiceTest
    {
        public SurveyService surveyService { get; set; }
        public IOptions<AppSettings> appSettings { get; set; }
        public SurveyServiceTest()
        {
            var configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", false)
            .Build();
            appSettings = Options.Create(configuration.GetSection("S8APP1").Get<AppSettings>());
            surveyService = new SurveyService(appSettings);
        }

        [Fact]
        public void GetSurveys_OnAvailableSurveys_ReturnsSurveys()
        {
            var expected = new SurveyRequestResponse();
            expected.Surveys = surveyService._surveys;
            var actual = surveyService.GetSurveys();
            actual.Should().BeEquivalentTo(expected);
        }

        [Fact]
        public void GetSurveys_OnNoAvailableSurveys_ReturnsError()
        {
            var error = "There are currently no surveys available";
            var expected = new SurveyRequestResponse();
            expected.Error = error;
            surveyService._surveys = new List<Survey>();
            var actual = surveyService.GetSurveys();
            actual.Should().BeEquivalentTo(expected);
        }

        [Fact]
        public void SubmitSurvey_OnValidSurvey_ReturnsTrue()
        {
            var path = Path.GetFullPath(appSettings.Value.persistencePath);
            File.WriteAllText(path, "{}");
            var answers = new List<QuestionAnswer>();
            answers.Add(new QuestionAnswer(1, "a"));
            answers.Add(new QuestionAnswer(2, "a"));
            answers.Add(new QuestionAnswer(3, "a"));
            answers.Add(new QuestionAnswer(4, "a"));
            SurveyResponse response = new SurveyResponse(0,answers);
            var actual = surveyService.SubmitSurvey(response, "admin");
            Assert.True(actual);
        }

        [Fact]
        public void SubmitSurvey_OnInvalidSurvey_ReturnsFalse()
        {
            var path = Path.GetFullPath(appSettings.Value.persistencePath);
            File.WriteAllText(path, "{}");
            var answers = new List<QuestionAnswer>();
            answers.Add(new QuestionAnswer(1, "bad"));
            answers.Add(new QuestionAnswer(2, "bad"));
            answers.Add(new QuestionAnswer(3, "bad"));
            answers.Add(new QuestionAnswer(4, "bad"));
            SurveyResponse response = new SurveyResponse(0, answers);
            var actual = surveyService.SubmitSurvey(response, "admin");
            Assert.False(actual);
        }

        [Fact]
        public void SubmitSurvey_OnValidDuplicateSurvey_ReturnsFalse()
        {
            var path = Path.GetFullPath(appSettings.Value.persistencePath);
            File.WriteAllText(path, "{\"admin\":[{\"SurveyId\":1,\"Responses\":[{\"QuestionId\":1,\"Answer\":\"a\"},{\"QuestionId\":2,\"Answer\":\"b\"},{\"QuestionId\":3,\"Answer\":\"c\"},{\"QuestionId\":4,\"Answer\":\"a\"}]},{\"SurveyId\":0,\"Responses\":[{\"QuestionId\":1,\"Answer\":\"a\"},{\"QuestionId\":2,\"Answer\":\"a\"},{\"QuestionId\":3,\"Answer\":\"a\"},{\"QuestionId\":4,\"Answer\":\"a\"}]}]}");
            var answers = new List<QuestionAnswer>();
            answers.Add(new QuestionAnswer(1, "a"));
            answers.Add(new QuestionAnswer(2, "a"));
            answers.Add(new QuestionAnswer(3, "a"));
            answers.Add(new QuestionAnswer(4, "a"));
            SurveyResponse response = new SurveyResponse(0, answers);
            var actual = surveyService.SubmitSurvey(response, "admin");
            Assert.False(actual);
        }

        [Fact]
        public void SubmitSurvey_OnValidSurveyWithExistingUser_ReturnsTrue()
        {
            var path = Path.GetFullPath(appSettings.Value.persistencePath);
            File.WriteAllText(path, "{\"admin\":[{\"SurveyId\":0,\"Responses\":[{\"QuestionId\":1,\"Answer\":\"a\"},{\"QuestionId\":2,\"Answer\":\"b\"},{\"QuestionId\":3,\"Answer\":\"c\"},{\"QuestionId\":4,\"Answer\":\"a\"}]}]}");
            var answers = new List<QuestionAnswer>();
            answers.Add(new QuestionAnswer(1, "a"));
            answers.Add(new QuestionAnswer(2, "a"));
            answers.Add(new QuestionAnswer(3, "a"));
            answers.Add(new QuestionAnswer(4, "a"));
            SurveyResponse response = new SurveyResponse(1, answers);
            var actual = surveyService.SubmitSurvey(response, "admin");
            Assert.True(actual);
        }

        [Fact]
        public void SubmitSurvey_NonExistantSurveyId_ReturnsFalse()
        {
            var answers = new List<QuestionAnswer>();
            answers.Add(new QuestionAnswer(1, "a"));
            answers.Add(new QuestionAnswer(2, "a"));
            answers.Add(new QuestionAnswer(3, "a"));
            answers.Add(new QuestionAnswer(4, "a"));
            SurveyResponse response = new SurveyResponse(15, answers);
            var actual = surveyService.SubmitSurvey(response, "admin");
            Assert.False(actual);
        }

        [Fact]
        public void SubmitSurvey_OnPartialCompletion_ReturnsFalse()
        {
            var answers = new List<QuestionAnswer>();
            answers.Add(new QuestionAnswer(1, "a"));
            answers.Add(new QuestionAnswer(2, "a"));
            SurveyResponse response = new SurveyResponse(0, answers);
            var actual = surveyService.SubmitSurvey(response, "admin");
            Assert.False(actual);
        }

        [Fact]
        public void SubmitSurvey_OnNonExistantQuestionId_ReturnsFalse()
        {
            var answers = new List<QuestionAnswer>();
            answers.Add(new QuestionAnswer(1, "a"));
            answers.Add(new QuestionAnswer(2, "a"));
            answers.Add(new QuestionAnswer(3, "a"));
            answers.Add(new QuestionAnswer(25, "a"));
            SurveyResponse response = new SurveyResponse(0, answers);
            var actual = surveyService.SubmitSurvey(response, "admin");
            Assert.False(actual);
        }
    }
}
