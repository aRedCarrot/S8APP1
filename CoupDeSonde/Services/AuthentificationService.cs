using CoupDeSonde.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace CoupDeSonde.Services
{
    public interface IAuthentificationService
    {
        LoginResponse Login(LoginRequest request);
        User GetByUsername(String username);
    }

    public class AuthentificationService : IAuthentificationService
    {
        private List<User> _users = new List<User>
        {
            new User("admin", "AQAAAAEAACcQAAAAEJYZUWmKW5xCOmiNHp0eBdzOIdoPUDYy7h0gTtVZwWfdBYVXTm2SUJwA2pyfGUEOdg=="),//admin
            new User("Mario", "AQAAAAEAACcQAAAAEMMYpvNr5RKfSY1LvF7DqExOmKNfeBpLvhz3u/HqQMk8yhFt3bYtqC74hgZTFQ0vmw=="),//Peach<3
            new User("TiBob", "AQAAAAEAACcQAAAAEJaQhT2VDKAlQt5xyU0Kk6oHWdh3wyBP5hIYMCuLnry/WIdufNBYXx71FfyZ0Ybt9w=="),//password
            new User("Secure", "AQAAAAEAACcQAAAAEC/VyigmKd8BYab13ZYgCk7WXSpmQwjwxsyWyQv6E7MQKDmpnEafmX+WO+CzHE80yA==")//1234
        };
        private readonly AppSettings? _appSettings;
        private readonly PasswordHasher<User> _hasher = new PasswordHasher<User>();
        public AuthentificationService()
        {
        }

        public AuthentificationService(IOptions<AppSettings> appSettings)
        {
            _appSettings = appSettings.Value;
            _users.ForEach(user => Console.WriteLine(_hasher.HashPassword(user, user.PasswordHash)));
        }

        public LoginResponse? Login(LoginRequest request)
        {
            var currentUser = _users.SingleOrDefault(user => user.Username == request.Username);

            if (currentUser == null || _hasher.VerifyHashedPassword(currentUser, currentUser.PasswordHash, request.Password) == PasswordVerificationResult.Failed)
                return null;

            return new LoginResponse(currentUser, generateJwtToken(currentUser));
        }

        public User GetByUsername(String username)
        {
            return _users.First(x => x.Username == username);
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
