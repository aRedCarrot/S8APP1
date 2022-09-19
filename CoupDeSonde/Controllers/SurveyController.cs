using CoupDeSonde.Models;
using CoupDeSonde.Services;
using Microsoft.AspNetCore.Mvc;

namespace CoupDeSonde.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class SurveyController : Controller
    {
        private readonly ISurveyService _surveyService;

        public SurveyController(ISurveyService surveyService)
        {
            _surveyService = surveyService;
        }

        [Authorize]
        [HttpGet("getSurveys")]
        public IActionResult GetSurveys()
        {
            return Ok(_surveyService.GetSurveys());
        }

        [Authorize]
        [HttpPost("submit")]
        public IActionResult SubmitSurvey(SurveyResponse response)
        {
            if (response == null || response.Responses == null)
                return BadRequest(new { message = "Response format is incorrect, check swagger documentation for more details" });

            var user = HttpContext.Items["User"] as User;
            if (_surveyService.SubmitSurvey(response, user.Username))
                return Ok();
            else
                return BadRequest(new { message = "You already submitted this survey or you improperly filled the survey." });
        }
    }
}
