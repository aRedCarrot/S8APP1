using CoupDeSonde.Controllers;
using CoupDeSonde.Models;
using CoupDeSonde.Services;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTests.Controllers
{
    public class SurveyControllerTest
    {
        public SurveyController _surveyController;
        public Mock<ISurveyService> _mockSurveyService;

        public SurveyControllerTest()
        {
            _mockSurveyService = new Mock<ISurveyService>();
            _surveyController = new SurveyController(_mockSurveyService.Object);

            var context = new DefaultHttpContext();
            context.Items["User"] = new User("Mario", "Hash");
            _surveyController.ControllerContext.HttpContext = context;
        }

        [Fact]
        public void GetSurveys_ReturnsOk()
        {
            var expectedResponse = new SurveyRequestResponse();
            expectedResponse.Surveys = new List<Survey> { createSurvey(1) , createSurvey(2) };
            _mockSurveyService.Setup(_ => _.GetSurveys()).Returns(expectedResponse);

            var actual = _surveyController.GetSurveys();
            actual.Should().BeEquivalentTo(new OkObjectResult(expectedResponse));
        }

        [Fact]
        public void Submit_FirstAnswer_ReturnsOk()
        {
            var answers = new List<QuestionAnswer>
            {
                new QuestionAnswer(1, "a"),
                new QuestionAnswer(2, "c")
            };

            var request= new SurveyResponse(1, answers);
            _mockSurveyService.Setup(_ => _.SubmitSurvey(request, "Mario")).Returns(true);

            var actual = _surveyController.SubmitSurvey(request);
            actual.Should().BeEquivalentTo(new OkResult());
        }

        [Fact]
        public void Submit_SecondAnswer_ReturnsOk()
        {
            var answers = new List<QuestionAnswer>
            {
                new QuestionAnswer(1, "a"),
                new QuestionAnswer(2, "c")
            };
            var request = new SurveyResponse(1, answers);

            _mockSurveyService.Setup(_ => _.SubmitSurvey(request, "Mario")).Returns(false);

            var actual = _surveyController.SubmitSurvey(request);
            actual.Should().BeEquivalentTo(new BadRequestObjectResult(new { message = "You already submitted this survey or you improperly filled the survey." }));
        }

        private Survey createSurvey(int id)
        {
            var testSurvey = new Survey("Test survey " + id);
            testSurvey.SurveyId = id;

            var question1 = new SurveyQuestion();
            question1.QuestionId = 1;
            question1.Prompt = "Answer this mate";
            question1.Options = new List<SurveyOption>
            {
                new SurveyOption("answer a", "a"),
                new SurveyOption("and b", "b"),
                new SurveyOption("Why not c", "c")
            };

            var question2 = new SurveyQuestion();
            question2.QuestionId = 2;
            question2.Prompt = "And now this";
            question2.Options = new List<SurveyOption>
            {
                new SurveyOption("answer 2a", "a"),
                new SurveyOption("and 2b", "b"),
                new SurveyOption("Why not 2c", "c")
            };


            testSurvey.SurveyQuestions = new List<SurveyQuestion> { question1, question2 };
            return testSurvey;
        }
    }
}
