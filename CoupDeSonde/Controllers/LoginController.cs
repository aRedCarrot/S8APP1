using CoupDeSonde.Models;
using CoupDeSonde.Services;
using Microsoft.AspNetCore.Mvc;

namespace CoupDeSonde.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class LoginController : ControllerBase
    {

        private readonly ILogger<LoginController> _logger;
        private IAuthentificationService _authService;

        public LoginController(ILogger<LoginController> logger, IAuthentificationService authService)
        {
            _logger = logger;
            _authService = authService;
        }

        [HttpPost(Name = "Login")]
        public IActionResult Login(LoginRequest model)
        {
            var response = _authService.Login(model);

            if (response == null)
                return BadRequest(new { message = "Username or password is incorrect" });

            return Ok(response);
        }
    }
}