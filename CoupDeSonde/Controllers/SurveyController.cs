using CoupDeSonde.Models;
using CoupDeSonde.Services;
using Microsoft.AspNetCore.Mvc;

namespace CoupDeSonde.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class SurveyController : Controller
    {
        private ISurveyService _surveyService;

        public SurveyController(ISurveyService surveyService)
        {
            _surveyService = surveyService;
        }

        [Authorize]
        [HttpGet("getSurvey/{surveyId}")]
        public IActionResult getSurvey(Int32 surveyId)
        {
            return Ok(_surveyService.GetSurvey(surveyId));
        }

        [Authorize]
        [HttpGet("getSurveys")]
        public IActionResult getSurveys()
        {
            return Ok(_surveyService.GetSurveys());
        }

        [Authorize]
        [HttpPost("submit")]
        public IActionResult submitSurvey(SurveyResponse response)
        {
            var user = HttpContext.Items["User"] as User;
            if (_surveyService.SubmitSurvey(response, user?.Username))
                return Ok();
            else
                return BadRequest(new { message = "The submitted survey response format was incorrect or your answer(s) was/were not a valid choice(s)" });
        }
    }
}
