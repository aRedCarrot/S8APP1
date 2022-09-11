using CoupDeSonde.Models;
using CoupDeSonde.Services;
using Microsoft.AspNetCore.Mvc;

namespace CoupDeSonde.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class SurveyController : Controller
    {
        private readonly ILogger<LoginController> _logger;
        private ISurveyService _surveyService;

        public SurveyController(ILogger<LoginController> logger, ISurveyService surveyService)
        {
            _logger = logger;
            _surveyService = surveyService;
        }

        [Authorize]
        [HttpGet("getSurvey")]
        public IActionResult getSurvey(SurveyRequest request)
        {
            return Ok(_surveyService.GetSurvey(request.Id));
        }
    }
}
