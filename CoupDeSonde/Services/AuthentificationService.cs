using CoupDeSonde.Models;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace CoupDeSonde.Services
{
    public interface IAuthentificationService
    {
        LoginResponse Login(LoginRequest model);
    }

    public class AuthentificationService : IAuthentificationService
    {
        private List<User> _users = new List<User>
        {
            new User("admin", "admin"),
            new User("Mario", "Peach<3"),
            new User("TiBob", "password"),
            new User("Secure", "1234")
        };
        private readonly AppSettings _appSettings;

        public AuthentificationService(IOptions<AppSettings> appSettings)
        {
            _appSettings = appSettings.Value;
        }

        public LoginResponse? Login(LoginRequest model)
        {
            var currentUser = _users.SingleOrDefault(user => user.Username == model.Username && user.Password == model.Password);

            if (currentUser != null )
                return new LoginResponse(currentUser, generateJwtToken(currentUser));
            else
                return null;
        }

        private string generateJwtToken(User user)
        {
            // generate token that is valid for 24 hours
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[] { new Claim("username", user.Username.ToString()) }),
                Expires = DateTime.UtcNow.AddHours(24),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}
