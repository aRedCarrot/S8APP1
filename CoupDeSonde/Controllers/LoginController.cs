using CoupDeSonde.Models;
using CoupDeSonde.Services;
using Microsoft.AspNetCore.Mvc;

namespace CoupDeSonde.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class LoginController : ControllerBase
    {
        private IAuthentificationService _authService;

        public LoginController(IAuthentificationService authService)
        {
            _authService = authService;
        }

        [HttpPost(Name = "Login")]
        public IActionResult Login(LoginRequest request)
        {
            if(request == null || request.Username == null || request.Password == null)
                return BadRequest(new { message = "Request format is incorrect, check swagger documentation for more details" });

            var response = _authService.Login(request);

            if (response == null)
                return BadRequest(new { message = "Username or password is incorrect" });

            return Ok(response);
        }
    }
}