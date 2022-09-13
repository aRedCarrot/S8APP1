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
            var response = _authService.Login(request);

            if (response == null)
                return BadRequest(new { message = "Username or password is incorrect" });

            return Ok(response);
        }
    }
}